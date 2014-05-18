using System;
namespace Andrei15193.Edesia.Models
{
	public class Product
	{
		public Product(string name, double price)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Cannot be empty or whitespace!", "name");

			if (price <= 0)
				throw new ArgumentException("Must be strictly positive!", "price");

			_name = name.Trim();
			_price = price;
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("Name");
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Cannot be empty or whitespace!", "Name");

				_name = value;
			}
		}
		public double Price
		{
			get
			{
				return _price;
			}
			set
			{
				if (value <= 0)
					throw new ArgumentException("Must be strictly positive!", "Price");

				_price = value;
			}
		}

		private string _name;
		private double _price;
	}
}