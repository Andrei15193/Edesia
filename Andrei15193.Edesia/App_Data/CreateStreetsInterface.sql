if exists(select name from sys.objects where type = 'P' and name = 'AddStreet')
	drop procedure AddStreet
if exists(select name from sys.objects where type = 'P' and name = 'RemoveStreet')
	drop procedure RemoveStreet
if exists(select name from sys.objects where type = 'P' and name = 'UpdateStreet')
	drop procedure UpdateStreet
if exists(select name from sys.objects where type = 'P' and name = 'ClearStreetsFromDeliveryZone')
	drop procedure ClearStreetsFromDeliveryZone
if exists(select name from sys.views where name = 'ActualStreets')
	drop view ActualStreets
if exists(select name from sys.views where name = 'UnmappedStreets')
	drop view UnmappedStreets
go

create procedure AddStreet @name nvarchar(100) as
	insert into Streets(name)
		values (@name)
go

create procedure RemoveStreet @name nvarchar(100) as
begin transaction
	if exists(select name
				  from Streets
				  where name = @name
						and isActual = 0
						and (deliveryZone is null and (select deliveryZone
														   from Streets
														    where name = @name and isActual = 1) is null)
							 or deliveryZone= (select top 1 deliveryZone
												   from Streets
												   where name = @name and isActual = 1))
		delete from Streets
			where name = @name and isActual = 1
	else
		update Streets
			set isActual = 0
			where name = @name and isActual = 1
commit
go

create procedure UpdateStreet @name nvarchar(100),
							  @deliveryZone nvarchar(100) as
begin
	update Streets
		set deliveryZone = (select top 1 id
								from DeliveryZones
								where isActual = 1 and name = @deliveryZone)
		where isActual = 1 and name = @name
end
go

create procedure ClearStreetsFromDeliveryZone @deliveryZone nvarchar(100) as
	update Streets
		set deliveryZone = null
		where isActual = 1 and deliveryZone = (select top 1 id
												   from DeliveryZones
												   where isActual = 1 and name = @deliveryZone)
go

create view ActualStreets as
	select name
		from Streets
		where isActual = 1
go

create view UnmappedStreets as
	select name
		from Streets
		where isActual = 1 and deliveryZone is null