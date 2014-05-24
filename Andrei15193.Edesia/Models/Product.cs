using System;
namespace Andrei15193.Edesia.Models
{
	public class Product
	{
		public Product(string name, double price, int capacity, Uri imageLocation)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Cannot be empty or whitespace!", "name");

			if (price <= 0)
				throw new ArgumentException("Must be strictly positive!", "price");

			if (capacity <= 0)
				throw new ArgumentException("Must be strictly positive!", "capacity");

			if (imageLocation == null)
				throw new ArgumentNullException("imageLocation");

			_name = name.Trim();
			_price = price;
			_capacity = capacity;
			_imageLocation = imageLocation;
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
		public int Capacity
		{
			get
			{
				return _capacity;
			}
		}
		public Uri ImageLocation
		{
			get
			{
				return _imageLocation;
			}
		}

		private string _name;
		private double _price;
		private readonly int _capacity;
		private readonly Uri _imageLocation;
	}
}