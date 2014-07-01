create table ApplicationUsers(
	eMail nvarchar(256),
	passwordHash varchar(512) not null,
	authenticationToken varchar(512) null,
	registrationToken varchar(50) null,
	firstName nvarchar(100) not null,
	lastName nvarchar(50) not null,
	transportCapacity float(8) null,
	isAdministrator bit default 0 not null,
	dateRegistered datetime not null,
	constraint pkApplicationUsers primary key (eMail)
)

create table Products(
	id int identity,
	name nvarchar(100),
	price float(8) not null,
	capacity float(8) not null,
	imageLocation nvarchar(1024) not null,
	isActual bit default 1 not null,
	constraint pkProducts primary key (id),
	constraint uniqueProductsEntry unique (name, price, capacity, isActual),
	constraint checkPositiveProductPrice check (price >= 0),
	constraint checkStrictlyPositiveProductCapactiy check (capacity > 0)
)

create table ShoppingCarts(
	owner nvarchar(256),
	product int,
	quantity int not null,
	constraint pkShoppingCarts primary key (owner, product),
	constraint fkShoppingCartOwner foreign key (owner) references ApplicationUsers(eMail),
	constraint fkShoppingCartProducts foreign key (product) references Products(id),
	constraint checkStrictlyPositiveProductQuantity check (quantity > 0)
)

create table DeliveryZones(
	id int identity,
	name nvarchar(100),
	colour varchar(7) not null,
	responsible nvarchar(256) null,
	isActual bit default 1 not null,
	constraint pkDeliveryZone primary key (id)
)
go
create trigger UniqueDeliveryZoneEntryTrigger
on DeliveryZones
after insert, update as
	if exists(select name
				  from DeliveryZones
				  where isActual = 1
				  group by name
				  having count(*) > 1)
	begin
		rollback
		raiserror('Duplicate delivery zone entry!', 18, 1)
	end
go

create table Streets(
	id int identity,
	name nvarchar(100),
	isActual bit default 1 not null,
	deliveryZone int null,
	constraint uniqueStreetsEntry unique (name, isActual, deliveryZone),
	constraint fkDeliveryZone foreign key (deliveryZone) references DeliveryZones(id)
)
go
create trigger StreetNameUniquenessTrigger
on Streets
after insert as
	if exists(select name
				  from Streets
				  where isActual = 1
				  group by name
				  having count(isActual) > 1)
	begin
		rollback
		raiserror('Name must be unique!', 18, 1)
	end
go

create table Orders(
	number int identity,
	datePlaced datetime not null,
	deliveryStreet nvarchar(100) not null,
	deliveryAddessDetails nvarchar(100) null,
	recipient nvarchar(256) not null,
	state int default 0,
	constraint pkOrders primary key (number),
	constraint fkRecipient foreign key (recipient) references ApplicationUsers(eMail)
)
go
create trigger OnlyOrderStateUpdatesAndNoDeletes
on Orders
instead of update, delete as
	if not update(number)
		update Orders
			set Orders.state = inserted.state
			from inserted
			where Orders.number = inserted.number
go
create trigger OrderDeliveryStreetForeignKeyTrigger
on Orders
after insert, update as
	if exists(select Orders.deliveryStreet
				  from Orders left join Streets
					  on Orders.deliveryStreet = Streets.name
				  where Orders.deliveryStreet = null)
	begin
		rollback
		raiserror('There is no corresponding deliveryStreet in Streets table!', 18, 1)
	end
go

create table OrderedProducts(
	product int,
	orderNumber int,
	quantity int not null,
	constraint pkOrderedProducts primary key (product, orderNumber),
	constraint fkProduct foreign key (product) references Products(id),
	constraint fkOrder foreign key (orderNumber) references Orders(number), 
	constraint checkStrictlyPositiveOrderedQuantity check (quantity > 0)
)
go

create table DeliveryTasks(
	number int identity,
	dateScheduled datetime not null,
	isCanceled bit default 0 not null,
	constraint pkDeliveryTasks primary key (number)
)

create table ScheduledOrders(
	deliveryTask int,
	orderNumber int,
	constraint pkScheduledOrders primary key (deliveryTask, orderNumber),
	constraint fkDeliveryTask foreign key (deliveryTask) references DeliveryTasks(number),
	constraint fkScheduledOrder foreign key (orderNumber) references Orders(number)
)