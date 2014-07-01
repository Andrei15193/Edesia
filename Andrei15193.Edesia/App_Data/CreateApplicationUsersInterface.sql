if exists(select name from sys.objects where type = 'P' and name = 'AddApplicationUser')
	drop procedure AddApplicationUser
if exists(select name from sys.objects where type = 'P' and name = 'EnrollEmployee')
	drop procedure EnrollEmployee
if exists(select name from sys.objects where type = 'P' and name = 'RemoveEmployee')
	drop procedure RemoveEmployee
if exists(select name from sys.objects where type = 'P' and name = 'EnrollAdministrator')
	drop procedure EnrollAdministrator
if exists(select name from sys.objects where type = 'P' and name = 'RemoveAdministrator')
	drop procedure RemoveAdministrator
if exists(select name from sys.objects where type = 'P' and name = 'ConfirmUser')
	drop procedure ConfirmUser
if exists(select name from sys.objects where type = 'P' and name = 'SetPasswordHash')
	drop procedure SetPasswordHash
if exists(select name from sys.objects where type = 'P' and name = 'SetAuthenticationToken')
	drop procedure SetAuthenticationToken
if exists(select name from sys.objects where type = 'IF' and name = 'GetUser')
	drop function GetUser
if exists(select name from sys.objects where type = 'IF' and name = 'GetUserByToken')
	drop function GetUserByToken
if exists(select name FROM sys.views where name = 'Employees')
	drop view Employees
if exists(select name FROM sys.views where name = 'Users')
	drop view Users
go

create procedure AddApplicationUser @eMail nvarchar(256),
									@passwordHash varchar(512),
									@registrationToken varchar(50),
									@firstName nvarchar(100),
									@lastName nvarchar(50),
									@dateRegistered datetime as
	insert into ApplicationUsers(eMail, passwordHash, registrationToken, firstName, lastName, dateRegistered)
		values (@eMail, @passwordHash, @registrationToken, @firstName, @lastName, @dateRegistered)
go

create procedure EnrollEmployee @eMail nvarchar(256),
								@transportCapacity float(8) as
	update ApplicationUsers
		set transportCapacity = @transportCapacity
		where eMail = @eMail
go

create procedure RemoveEmployee @eMail nvarchar(256) as
	update ApplicationUsers
		set transportCapacity = null
		where eMail = @eMail
go

create procedure EnrollAdministrator @eMail nvarchar(256) as
	update ApplicationUsers
		set isAdministrator = 1
		where eMail = @eMail
go

create procedure RemoveAdministrator @eMail nvarchar(256) as
	update ApplicationUsers
		set isAdministrator = 0
		where eMail = @eMail
go

create procedure ConfirmUser @eMail nvarchar(256),
							 @registrationToken varchar(50),
							 @confirmed bit out as
begin transaction
	if exists(select eMail
				  from ApplicationUsers
				  where eMail = @eMail and registrationToken = @registrationToken)
	begin
		update ApplicationUsers
			set registrationToken = null
			where eMail = @eMail and registrationToken = @registrationToken 
		set @confirmed = 1
	end
	else
		set @confirmed = 0
commit
go

create procedure SetPasswordHash @eMail nvarchar(256), @passwordHash varchar(512) as
	update ApplicationUsers
		set passwordHash = @passwordHash
		where eMail = @eMail and registrationToken is null
go

create procedure SetAuthenticationToken @eMail nvarchar(256), @authenticationToken varchar(512) as
	update ApplicationUsers
		set authenticationToken = @authenticationToken
		where eMail = @eMail and registrationToken is null
go

create function GetUser(@eMail nvarchar(256), @passwordHash varchar(512)) returns table as
	return select eMail, firstName, lastName, transportCapacity, isAdministrator, dateRegistered
			   from ApplicationUsers
			   where eMail = @Email
					 and passwordHash = @passwordHash
					 and registrationToken is null
go

create function GetUserByToken(@eMail nvarchar(256), @authenticationToken varchar(512)) returns table as
	return select eMail, firstName, lastName, transportCapacity, isAdministrator, dateRegistered
			   from ApplicationUsers
			   where eMail = @Email
					 and authenticationToken = @authenticationToken
					 and registrationToken is null
go

create view Employees as
	select eMail, firstName, lastName, transportCapacity, isAdministrator, dateRegistered
		from ApplicationUsers
		where transportCapacity is not null
go

create view Users as
	select eMail, firstName, lastName, transportCapacity, isAdministrator, dateRegistered
		from ApplicationUsers
		where registrationToken is null