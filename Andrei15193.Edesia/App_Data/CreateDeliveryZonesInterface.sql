if exists(select name from sys.objects where type = 'P' and name = 'AddDeliveryZone')
	drop procedure AddDeliveryZone
if exists(select name from sys.objects where type = 'P' and name = 'UpdateDeliveryZone')
	drop procedure UpdateDeliveryZone
if exists(select name from sys.objects where type = 'P' and name = 'RemoveDeliveryZone')
	drop procedure RemoveDeliveryZone
if exists(select name from sys.objects where type = 'IF' and name = 'GetDeliveryZone')
	drop function GetDeliveryZone
if exists(select name from sys.views where name = 'ActualDeliveryZones')
	drop view ActualDeliveryZones
go

create procedure AddDeliveryZone @name nvarchar(100),
								 @colour varchar(7),
								 @responsible nvarchar(256) as
if (@responsible is null
		or exists(select eMail
					  from Users
					  where eMail = @responsible and transportCapacity is not null))
		insert into DeliveryZones(name, colour, responsible)
			values (@name, @colour, @responsible)
	else
		raiserror('The responsible must be an employee!', 18, 1)
go

create procedure UpdateDeliveryZone @name nvarchar(100),
									@oldName nvarchar(100),
									@colour varchar(7),
									@responsible nvarchar(256) as
	if (@responsible is null
		or exists(select eMail
					  from Users
					  where eMail = @responsible and transportCapacity is not null))
		update DeliveryZones
			set name = @name,
				colour = @colour,
				responsible = @responsible
			where name = @oldName and isActual = 1
	else
		raiserror('The responsible must be an employee!', 18, 1)
go

create procedure RemoveDeliveryZone @name nvarchar(100) as
	update DeliveryZones
		set isActual = 0
		where name = @name and isActual = 1
go

create function GetDeliveryZone(@name nvarchar(100)) returns table as
	return select DeliveryZones.name, DeliveryZones.colour, Users.*, Streets.name as streetName
			   from (select DeliveryZones.id, DeliveryZones.name, DeliveryZones.colour, DeliveryZones.responsible
						 from DeliveryZones
						 where name = @name and isActual = 1) as DeliveryZones
					left join Users
						on DeliveryZones.responsible = Users.eMail
					left join Streets
						on Streets.deliveryZone = DeliveryZones.id
go

create view ActualDeliveryZones as
	select DeliveryZones.name, DeliveryZones.colour,
		   Streets.name as streetName,
		   Users.*
		from (select id, name, colour, responsible
				  from DeliveryZones
				  where DeliveryZones.isActual = 1) as DeliveryZones
			left join Users
				on DeliveryZones.responsible = Users.eMail
			left join (select name, deliveryZone
							from Streets
							where isActual = 1) as Streets
				on DeliveryZones.id = Streets.deliveryZone