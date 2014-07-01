if exists(select name from sys.objects where type = 'P' and name = 'AddDeliveryTask')
	drop procedure AddDeliveryTask
if exists(select name from sys.objects where type = 'P' and name = 'UpdateDeliveryTask')
	drop procedure UpdateDeliveryTask
if exists(select name from sys.objects where type = 'P' and name = 'AddScheduledOrder')
	drop procedure AddScheduledOrder
if exists(select name from sys.objects where type = 'IF' and name = 'GetDeliveryTaskHavingOrdersInState')
	drop function GetDeliveryTaskHavingOrdersInState
if exists(select name from sys.objects where type = 'IF' and name = 'GetDeliveryTaskHavingOrdersInStateForEmployee')
	drop function GetDeliveryTaskHavingOrdersInStateForEmployee
if exists(select name from sys.objects where type = 'IF' and name = 'GetCanceledDeliveryTasksForEmployee')
	drop function GetCanceledDeliveryTasksForEmployee
if exists(select name from sys.objects where type = 'IF' and name = 'GetDeliveryTaskByNumber')
	drop function GetDeliveryTaskByNumber
if exists(select name from sys.views where name = 'CanceledDeliveryTasks')
	drop view CanceledDeliveryTasks
go

create procedure AddDeliveryTask @dateScheduled datetime,
								 @number int out as
begin
	insert into DeliveryTasks (dateScheduled)
		values (@dateScheduled)
	set @number = SCOPE_IDENTITY()
end
go

create procedure UpdateDeliveryTask @number int,
									@isCanceled bit as
begin
	update DeliveryTasks
		set isCanceled = @isCanceled
		where number = @number
end
go

create procedure AddScheduledOrder @deliveryTask int,
								   @orderNumber int as
	insert into ScheduledOrders(deliveryTask, orderNumber)
		values (@deliveryTask, @orderNumber)
go

create function GetDeliveryTaskHavingOrdersInState(@state int) returns table as
	return select DeliveryTasks.number as deliveryTaskNumber, DeliveryTasks.dateScheduled, DeliveryTasks.isCanceled,
				  Orders.number, Orders.datePlaced, Orders.deliveryStreet, Orders.deliveryAddessDetails, Orders.state,
				  Users.*,
				  DeliveryZones.name as deliveryZoneName, DeliveryZones.colour as deliveryZoneColour, DeliveryZoneStreets.name as deliveryZoneStreetName,
				  Employees.dateRegistered as employeeDateRegistered, Employees.eMail as employeeEMail, Employees.firstName as employeeFirstName, Employees.lastName as employeeLastName, Employees.transportCapacity as employeeTransportCapacity, Employees.isAdministrator as employeeIsAdministrator,
				  Products.name, Products.price, Products.capacity, Products.imageLocation,
				  OrderedProducts.quantity
			   from (select *
						 from DeliveryTasks
						 where isCanceled = 0) as DeliveryTasks
				   inner join (select ScheduledOrders.deliveryTask as deliveryTaskNumber
								   from ScheduledOrders
									   left join (select *
													  from Orders
													  where state = @state) as Orders
										   on ScheduledOrders.orderNumber = Orders.number
								   group by ScheduledOrders.deliveryTask
								   having sum(case when Orders.number is null then 1 else 0 end) = 0) as ValidDeliveryTasks
					   on DeliveryTasks.number = ValidDeliveryTasks.deliveryTaskNumber
				   inner join ScheduledOrders
					   on DeliveryTasks.number = ScheduledOrders.deliveryTask
				   inner join Orders
					   on ScheduledOrders.orderNumber = Orders.number
				   inner join Streets
					   on Orders.deliveryStreet = Streets.name
				   inner join DeliveryZones
					   on Streets.deliveryZone = DeliveryZones.id
				   inner join Employees 
					   on DeliveryZones.responsible = Employees.eMail
				   inner join Streets as DeliveryZoneStreets
					   on DeliveryZoneStreets.deliveryZone = DeliveryZones.id
				   inner join Users
					   on Orders.recipient = Users.eMail
				   inner join OrderedProducts
					   on Orders.number = OrderedProducts.orderNumber
				   inner join Products
					   on OrderedProducts.product = Products.id
go

create function GetDeliveryTaskHavingOrdersInStateForEmployee(@state int, @eMail nvarchar(256)) returns table as
	return select DeliveryTasks.number as deliveryTaskNumber, DeliveryTasks.dateScheduled, DeliveryTasks.isCanceled,
				  Orders.number, Orders.datePlaced, Orders.deliveryStreet, Orders.deliveryAddessDetails, Orders.state,
				  Users.*,
				  DeliveryZones.name as deliveryZoneName, DeliveryZones.colour as deliveryZoneColour, DeliveryZoneStreets.name as deliveryZoneStreetName,
				  Products.name, Products.price, Products.capacity, Products.imageLocation,
				  OrderedProducts.quantity
			   from (select *
						 from DeliveryTasks
						 where isCanceled = 0) as DeliveryTasks
				   inner join (select ScheduledOrders.deliveryTask as deliveryTaskNumber
								   from ScheduledOrders
									   left join (select *
													  from Orders
													  where state = @state) as Orders
										   on ScheduledOrders.orderNumber = Orders.number
								   group by ScheduledOrders.deliveryTask
								   having sum(case when Orders.number is null then 1 else 0 end) = 0) as ValidDeliveryTasks
					   on DeliveryTasks.number = ValidDeliveryTasks.deliveryTaskNumber
				   inner join ScheduledOrders
					   on DeliveryTasks.number = ScheduledOrders.deliveryTask
				   inner join Orders
					   on ScheduledOrders.orderNumber = Orders.number
				   inner join Streets
					   on Orders.deliveryStreet = Streets.name
				   inner join (select *
								   from DeliveryZones
								   where DeliveryZones.responsible = @eMail) as DeliveryZones
					   on Streets.deliveryZone = DeliveryZones.id
				   inner join Employees 
					   on DeliveryZones.responsible = Employees.eMail
				   inner join Streets as DeliveryZoneStreets
					   on DeliveryZoneStreets.deliveryZone = DeliveryZones.id
				   inner join Users
					   on Orders.recipient = Users.eMail
				   inner join OrderedProducts
					   on Orders.number = OrderedProducts.orderNumber
				   inner join Products
					   on OrderedProducts.product = Products.id
go

create view CanceledDeliveryTasks as
	select DeliveryTasks.number as deliveryTaskNumber, DeliveryTasks.dateScheduled, DeliveryTasks.isCanceled,
		   Orders.number, Orders.datePlaced, Orders.deliveryStreet, Orders.deliveryAddessDetails, Orders.state,
		   Users.*,
		   DeliveryZones.name as deliveryZoneName, DeliveryZones.colour as deliveryZoneColour, DeliveryZoneStreets.name as deliveryZoneStreetName,
		   Employees.dateRegistered as employeeDateRegistered, Employees.eMail as employeeEMail, Employees.firstName as employeeFirstName, Employees.lastName as employeeLastName, Employees.transportCapacity as employeeTransportCapacity, Employees.isAdministrator as employeeIsAdministrator,
		   Products.name, Products.price, Products.capacity, Products.imageLocation,
		   OrderedProducts.quantity
	   from (select *
				   from DeliveryTasks
				   where isCanceled = 1) as DeliveryTasks
			inner join ScheduledOrders
				on DeliveryTasks.number = ScheduledOrders.deliveryTask
			inner join Orders
				on ScheduledOrders.orderNumber = Orders.number
			inner join Streets
				on Orders.deliveryStreet = Streets.name
			inner join DeliveryZones
				on Streets.deliveryZone = DeliveryZones.id
			inner join Employees 
				on DeliveryZones.responsible = Employees.eMail
			inner join Streets as DeliveryZoneStreets
				on DeliveryZoneStreets.deliveryZone = DeliveryZones.id
			inner join Users
				on Orders.recipient = Users.eMail
			inner join OrderedProducts
				on Orders.number = OrderedProducts.orderNumber
			inner join Products
				on OrderedProducts.product = Products.id
go

create function GetCanceledDeliveryTasksForEmployee(@eMail nvarchar(256)) returns table as
	return select DeliveryTasks.number as deliveryTaskNumber, DeliveryTasks.dateScheduled, DeliveryTasks.isCanceled,
					  Orders.number, Orders.datePlaced, Orders.deliveryStreet, Orders.deliveryAddessDetails, Orders.state,
					  Users.*,
					  DeliveryZones.name as deliveryZoneName, DeliveryZones.colour as deliveryZoneColour, DeliveryZoneStreets.name as deliveryZoneStreetName,
					  Employees.dateRegistered as employeeDateRegistered, Employees.eMail as employeeEMail, Employees.firstName as employeeFirstName, Employees.lastName as employeeLastName, Employees.transportCapacity as employeeTransportCapacity, Employees.isAdministrator as employeeIsAdministrator,
					  Products.name, Products.price, Products.capacity, Products.imageLocation,
					  OrderedProducts.quantity
			  from (select *
						  from DeliveryTasks
						  where isCanceled = 1) as DeliveryTasks
				  inner join ScheduledOrders
					  on DeliveryTasks.number = ScheduledOrders.deliveryTask
				  inner join Orders
					  on ScheduledOrders.orderNumber = Orders.number
				  inner join Streets
					  on Orders.deliveryStreet = Streets.name
				  inner join (select *
								   from DeliveryZones
								   where DeliveryZones.responsible = @eMail) as DeliveryZones
					  on Streets.deliveryZone = DeliveryZones.id
				  inner join Employees
					  on DeliveryZones.responsible = Employees.eMail
				  inner join Streets as DeliveryZoneStreets
					  on DeliveryZoneStreets.deliveryZone = DeliveryZones.id
				  inner join Users
					  on Orders.recipient = Users.eMail
				  inner join OrderedProducts
					  on Orders.number = OrderedProducts.orderNumber
				  inner join Products
					  on OrderedProducts.product = Products.id
go

create function GetDeliveryTaskByNumber(@number int) returns table as
	return select DeliveryTasks.number as deliveryTaskNumber, DeliveryTasks.dateScheduled, DeliveryTasks.isCanceled,
				  Orders.number, Orders.datePlaced, Orders.deliveryStreet, Orders.deliveryAddessDetails, Orders.state,
				  Users.*,
				  DeliveryZones.name as deliveryZoneName, DeliveryZones.colour as deliveryZoneColour, DeliveryZoneStreets.name as deliveryZoneStreetName,
				  Employees.dateRegistered as employeeDateRegistered, Employees.eMail as employeeEMail, Employees.firstName as employeeFirstName, Employees.lastName as employeeLastName, Employees.transportCapacity as employeeTransportCapacity, Employees.isAdministrator as employeeIsAdministrator,
				  Products.name, Products.price, Products.capacity, Products.imageLocation,
				  OrderedProducts.quantity
			   from (select *
						 from DeliveryTasks
						 where number = @number) as DeliveryTasks
				   inner join ScheduledOrders
					   on DeliveryTasks.number = ScheduledOrders.deliveryTask
				   inner join Orders
					   on ScheduledOrders.orderNumber = Orders.number
				   inner join Streets
					   on Orders.deliveryStreet = Streets.name
				   inner join DeliveryZones
					   on Streets.deliveryZone = DeliveryZones.id
				   inner join Employees 
					   on DeliveryZones.responsible = Employees.eMail
				   inner join Streets as DeliveryZoneStreets
					   on DeliveryZoneStreets.deliveryZone = DeliveryZones.id
				   inner join Users
					   on Orders.recipient = Users.eMail
				   inner join OrderedProducts
					   on Orders.number = OrderedProducts.orderNumber
				   inner join Products
					   on OrderedProducts.product = Products.id