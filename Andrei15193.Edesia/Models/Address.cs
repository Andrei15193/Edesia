using System;
namespace Andrei15193.Edesia.Models
{
	public class Address
	{
		public Address(string street, City city, County county)
		{
			if (street == null)
				throw new ArgumentNullException("street");
			_street = street;
			City = city;
			County = county;
		}

		public City City
		{
			get;
			set;
		}
		public County County
		{
			get;
			set;
		}
		public string Street
		{
			get
			{
				return _street;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("Street");
				_street = value;
			}
		}

		private string _street;
	}
}