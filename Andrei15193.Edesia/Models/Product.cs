using System;
namespace Andrei15193.Edesia.Models
{
	public class Product
	{
		public Product(string name, double price, DateTime dateAdded, DateTime? dateRemoved = null)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Cannot be empty or whitespace!", "name");

			if (price <= 0)
				throw new ArgumentException("Must be strictly positive!", "price");

			_name = name.Trim();
			_price = price;

			if (dateRemoved.HasValue)
				if (dateRemoved.Value == dateAdded)
					throw new ArgumentException("dateAdded and dateRemoved must be distict!");
				else
					if (dateRemoved.Value < dateAdded)
					{
						_dateAdded = dateRemoved.Value;
						DateRemoved = dateRemoved;
					}

			_dateAdded = dateAdded;
			DateRemoved = dateRemoved;
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
		public DateTime DateAdded
		{
			get
			{
				return _dateAdded;
			}
		}
		public DateTime? DateRemoved
		{
			get;
			set;
		}

		private double _price;
		private string _name;
		private readonly DateTime _dateAdded;
	}
}