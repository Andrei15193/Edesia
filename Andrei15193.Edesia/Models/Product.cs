using System;
using System.Collections.Generic;
namespace Andrei15193.Edesia.Models
{
	public class Product
	{
		public Product(string name, double price, double capacity, Uri imageLocation)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Cannot be empty or whitespace!", "name");

			if (price < 0)
				throw new ArgumentException("Must be positive!", "price");

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
		public double Capacity
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
		public static IEqualityComparer<Product> IdentityComparer
		{
			get
			{
				return _identityComparer;
			}
		}

		private string _name;
		private double _price;
		private readonly double _capacity;
		private readonly Uri _imageLocation;
		private static readonly IEqualityComparer<Product> _identityComparer = new ProductIdentityComparer();

		private sealed class ProductIdentityComparer
			: IEqualityComparer<Product>
		{
			#region IEqualityComparer<Product> Members
			public bool Equals(Product one, Product another)
			{
				if (one == null)
					return (another == null);
				else
					return (another != null
							&& string.Equals(one._name, another._name, StringComparison.OrdinalIgnoreCase));
			}
			public int GetHashCode(Product value)
			{
				if (value == null)
					throw new ArgumentNullException("value");

				return value._name.GetHashCode();
			}
			#endregion
		}
	}
}