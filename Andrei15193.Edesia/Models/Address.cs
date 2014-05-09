using System;
namespace Andrei15193.Edesia.Models
{
	public class Address
	{
		public Address(string street, string details = null)
		{
			if (street == null)
				throw new ArgumentNullException("street");
			if (string.IsNullOrEmpty(street) || string.IsNullOrWhiteSpace(street))
				throw new ArgumentException("Cannot be empty or whitespace!", "street");

			_street = street;

			if (string.IsNullOrEmpty(details) || string.IsNullOrWhiteSpace(details))
				_details = null;
			else
				_details = details;
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
				if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Cannot be empty or whitespace!", "Street");

				_street = value;
			}
		}
		public string Details
		{
			get
			{
				return _details;
			}
			set
			{
				if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
					_details = null;
				else
					_details = value;
			}
		}
		public override string ToString()
		{
			if (_details == null)
				return _street;
			else
				return string.Join(", ", _street, _details);
		}

		private string _street;
		private string _details;
	}
}