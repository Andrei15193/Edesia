if exists(select name from sys.objects where type = 'P' and name = 'AddProduct')
	drop procedure AddProduct
if exists(select name from sys.objects where type = 'P' and name = 'RemoveProduct')
	drop procedure RemoveProduct
if exists(select name from sys.objects where type = 'IF' and name = 'GetProduct')
	drop function GetProduct
if exists(select name FROM sys.views where name = 'ActualProducts')
	drop view ActualProducts
go

create procedure AddProduct @name nvarchar(100),
							@price float(8),
							@capacity float(8),
							@imageLocation nvarchar(1024) as
begin transaction
	if not exists(select name
					  from Products
					  where name = @name)
		insert into Products(name, price, capacity, imageLocation)
			values (@name, @price, @capacity, @imageLocation)
	else
		update Products
			set isActual = 1
			where name = @name
commit
go

create procedure RemoveProduct @name nvarchar(100) as
	update Products
		set isActual = 0
		where name = @name
go

create view ActualProducts as
	select id, name, price, capacity, imageLocation
		from Products
		where isActual = 1
go

create function GetProduct(@name nvarchar(100)) returns table as
	return select *
			   from ActualProducts
			   where name = @name