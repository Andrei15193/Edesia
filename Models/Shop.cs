namespace Andrei15193.Edesia.Models
{
	public class Shop
	{
		public Shop(string shopName, string shopAddress)
		{
			ShopName = shopName;
			ShopAddress = shopAddress;
		}

		public int ShopId
		{
			get;
			set;
		}
		public string ShopName
		{
			get;
			private set;
		}
		public string ShopAddress
		{
			get;
			private set;
		}
	}
}