if exists(select * from sys.objects where type = 'T' and name = 'UniqueDeliveryZoneEntryTrigger')
	drop trigger UniqueDeliveryZoneEntryTrigger
if exists(select * from sys.objects where type = 'T' and name = 'OnlyOrderStateUpdatesAndNoDeletes')
	drop trigger OnlyOrderStateUpdatesAndNoDeletes
if exists(select * from sys.objects where type = 'T' and name = 'OrderDeliveryStreetForeignKeyTrigger')
	drop trigger OrderDeliveryStreetForeignKeyTrigger
if exists(select * from sys.objects where type = 'T' and name = 'StreetNameUniquenessTrigger')
	drop trigger StreetNameUniquenessTrigger

if exists(select * from sys.objects where type = 'U' and name = 'ScheduledOrders')
	drop table ScheduledOrders
if exists(select * from sys.objects where type = 'U' and name = 'DeliveryTasks')
	drop table DeliveryTasks
if exists(select * from sys.objects where type = 'U' and name = 'OrderedProducts')
	drop table OrderedProducts
if exists(select * from sys.objects where type = 'U' and name = 'Orders')
	drop table Orders
if exists(select * from sys.objects where type = 'U' and name = 'Streets')
	drop table Streets
if exists(select * from sys.objects where type = 'U' and name = 'DeliveryZones')
	drop table DeliveryZones
if exists(select * from sys.objects where type = 'U' and name = 'ShoppingCarts')
	drop table ShoppingCarts
if exists(select name from sys.objects where type = 'U' and name = 'Products')
	drop table Products
if exists(select * from sys.objects where type = 'U' and name = 'ApplicationUsers')
	drop table ApplicationUsers