if exists(select name from sys.objects where type = 'P' and name = 'ClearShoppingCart')
	drop procedure ClearShoppingCart
if exists(select name from sys.objects where type = 'P' and name = 'AddToCart')
	drop procedure AddToCart
if exists(select name from sys.objects where type = 'P' and name = 'UpdateCart')
	drop procedure UpdateCart
if exists(select name from sys.objects where type = 'P' and name = 'RemoveFromCart')
	drop procedure RemoveFromCart
if exists(select name from sys.objects where type = 'IF' and name = 'GetShoppingCart')
	drop function GetShoppingCart
go

create procedure ClearShoppingCart @owner nvarchar(256) as
	delete from ShoppingCarts
		where owner = @owner
go

create procedure AddToCart @owner nvarchar(256),
						   @product nvarchar(100),
						   @quantity int as
begin transaction
	if exists(select quantity
				  from ShoppingCarts
				  where owner = @owner and quantity = @quantity)
		update ShoppingCarts
			set quantity = quantity + @quantity
			where @owner = owner and product = (select top 1 id
													from ActualProducts
													where name = @product)
	else
		insert into ShoppingCarts(owner, product, quantity)
			values(@owner,
				   (select top 1 id
						from ActualProducts
						where name = @product),
				   @quantity)
commit
go

create procedure UpdateCart @owner nvarchar(256),
							@product nvarchar(100),
							@quantity int as
begin transaction
	declare @productId as int = (select top 1 id
									 from ActualProducts
									 where name = @product)
	if exists(select quantity
				  from ShoppingCarts
				  where owner = @owner and product = @productId)
		update ShoppingCarts
			set quantity = @quantity
			where @owner = owner and product = @productId
	else
		insert into ShoppingCarts(owner, product, quantity)
			values(@owner, @productId, @quantity)
commit
go

create procedure RemoveFromCart @owner nvarchar(256),
								@product nvarchar(100) as
	delete from ShoppingCarts
			   where owner = @owner and product = (select top 1 id
													   from ActualProducts
													   where name = @product)
go

create function GetShoppingCart(@owner nvarchar(256)) returns table as
	return select ActualProducts.name, ActualProducts.price, ActualProducts.capacity, ActualProducts.imageLocation,
				  ShoppingCarts.quantity
			   from (select ShoppingCarts.product, ShoppingCarts.quantity
						 from ShoppingCarts
						 where owner = @owner) as ShoppingCarts
				   inner join ActualProducts
					   on ShoppingCarts.product = ActualProducts.id