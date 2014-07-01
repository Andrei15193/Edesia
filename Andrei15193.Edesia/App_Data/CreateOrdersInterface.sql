if exists(select name from sys.objects where type = 'P' and name = 'AddOrder')
	drop procedure AddOrder
if exists(select name from sys.objects where type = 'P' and name = 'AddOrderedProduct')
	drop procedure AddOrderedProduct
if exists(select name from sys.objects where type = 'P' and name = 'UpdateOrder')
	drop procedure UpdateOrder
if exists(select name from sys.objects where type = 'IF' and name = 'GetOrders')
	drop function GetOrders
if exists(select name from sys.objects where type = 'IF' and name = 'GetOrders')
	drop function GetOrdersForRecipient
go

create procedure AddOrder @datePlaced datetime,
						  @deliveryStreet nvarchar(100),
						  @deliveryAddessDetails nvarchar(100),
						  @recipient nvarchar(256),
						  @number int out as
begin
	insert into Orders(datePlaced, deliveryStreet, deliveryAddessDetails, recipient)
		values (@datePlaced, @deliveryStreet, @deliveryAddessDetails, @recipient)
	set @number = SCOPE_IDENTITY()
end
go

create procedure UpdateOrder @number int,
							 @state int as
	update Orders
		set state = @state
		where number = @number
go

create procedure AddOrderedProduct @product nvarchar(100),
								   @quantity int,
								   @orderNumber int as
	insert into OrderedProducts(product, quantity, orderNumber)
		values ((select top 1 id
					 from Products
					 where name = @product and isActual = 1),
				@quantity,
				@orderNumber)
go

create function GetOrders(@state int) returns table as
	return select Orders.datePlaced, Orders.deliveryAddessDetails, Orders.deliveryStreet, Orders.number, Orders.recipient, Orders.state,
				  Users.*,
				  OrderedProducts.quantity,
				  Products.name, Products.price, Products.capacity, Products.imageLocation
			   from (select *
						 from Orders
						 where Orders.state = @state) as Orders
				   inner join Users
					   on Orders.recipient = Users.eMail
				   inner join OrderedProducts
					   on Orders.number = OrderedProducts.orderNumber
				   inner join Products
					   on OrderedProducts.product = Products.id
go

create function GetOrdersForRecipient(@state int, @recipient nvarchar(256)) returns table as
	return select Orders.datePlaced, Orders.deliveryAddessDetails, Orders.deliveryStreet, Orders.number, Orders.recipient, Orders.state,
				  OrderedProducts.quantity,
				  Products.name, Products.price, Products.capacity, Products.imageLocation
			   from (select *
						 from Orders
						 where Orders.state = @state and recipient = @recipient) as Orders
				   inner join OrderedProducts
					   on Orders.number = OrderedProducts.orderNumber
				   inner join Products
					   on OrderedProducts.product = Products.id
go