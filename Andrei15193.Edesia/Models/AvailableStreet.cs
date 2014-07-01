using System;
namespace Andrei15193.Edesia.Models
{
	public struct AvailableStreet
	{
		public AvailableStreet(string name, bool isAssociated)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Cannot be empty or white sapce!", "name");

			_name = name.Trim();
			_isAssociated = isAssociated;
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}
		public bool IsAssociated
		{
			get
			{
				return _isAssociated;
			}
		}

		private readonly bool _isAssociated;
		private readonly string _name;
	}
}