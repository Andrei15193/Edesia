using System;
namespace Andrei15193.Edesia.Models
{
	public class DetailedAddress
	{
		public DetailedAddress(string address, string details = null)
		{
			if (address == null)
				throw new ArgumentNullException("address");
			if (string.IsNullOrEmpty(address) || string.IsNullOrWhiteSpace(address))
				throw new ArgumentException("Cannot be empty or whitespace!", "address");

			_address = address;

			if (string.IsNullOrEmpty(details) || string.IsNullOrWhiteSpace(details))
				_details = null;
			else
				_details = details;
		}

		public string Address
		{
			get
			{
				return _address;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("Address");
				if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Cannot be empty or whitespace!", "Address");

				_address = value;
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
				return _address;
			else
				return string.Join(", ", _address, _details);
		}

		private string _address;
		private string _details;
	}
}