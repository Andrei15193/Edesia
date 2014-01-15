namespace Andrei15193.Edesia.Models
{
	public class Offer
	{
		public Offer(Shop shop, Product product, uint price)
		{
			Price = price;
			Shop = shop;
			Product = product;
		}

		public uint Price
		{
			get;
			private set;
		}
		public Shop Shop
		{
			get;
			private set;
		}
		public Product Product
		{
			get;
			private set;
		}
	}
}