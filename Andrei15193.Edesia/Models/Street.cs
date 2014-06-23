using System;
using System.Collections.Generic;
namespace Andrei15193.Edesia.Models
{
	public class Street
		: IComparable<Street>
	{
		public Street(string name, DateTime? dateRemoved = null)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Cannot be empty or whitespace!", "name");

			_name = name.Trim();
			_dateRemoved = dateRemoved;
		}

		#region IComparable<Street> Members
		public int CompareTo(Street other)
		{
			if (other == null)
				throw new ArgumentNullException("other");

			return string.Compare(_name, other._name, StringComparison.OrdinalIgnoreCase);
		}
		#endregion
		public override string ToString()
		{
			return _name;
		}
		public string Name
		{
			get
			{
				return _name;
			}
		}
		public DateTime? DateRemoved
		{
			get
			{
				return _dateRemoved;
			}
		}
		public static IEqualityComparer<Street> IdentityComparer
		{
			get
			{
				return _identityComparer;
			}
		}

		private readonly string _name;
		private readonly DateTime? _dateRemoved;
		private readonly static IEqualityComparer<Street> _identityComparer = new StreetIdentityComparer();

		private sealed class StreetIdentityComparer
			: IEqualityComparer<Street>
		{
			#region IEqualityComparer<Street> Members
			public bool Equals(Street one, Street another)
			{
				if (one == null)
					return (another == null);
				else
					return (another != null
							&& string.Equals(one._name, another._name, StringComparison.OrdinalIgnoreCase)
							&& one._dateRemoved.Equals(another._dateRemoved));
			}
			public int GetHashCode(Street value)
			{
				if (value == null)
					throw new ArgumentNullException("value");

				return (value._name.GetHashCode() ^ value._dateRemoved.GetHashCode());
			}
			#endregion
		}
	}
}