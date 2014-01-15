namespace Andrei15193.Edesia.Models
{
	public class Product
	{
		public Product(string productName, string producerName)
		{
			ProductName = productName;
			ProducerName = producerName;
		}

		public int ProductId
		{
			get;
			set;
		}
		public string ProductName
		{
			get;
			private set;
		}
		public string ProducerName
		{
			get;
			private set;
		}
	}
}