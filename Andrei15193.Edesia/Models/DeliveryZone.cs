﻿using System;
using System.Collections.Generic;
namespace Andrei15193.Edesia.Models
{
	public class DeliveryZone
	{
		public DeliveryZone(string name, Colour colour, DateTime dateAdded, IEnumerable<Street> streets)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Cannot be empty or whitespace!", "name");

			_name = name;
			_colour = colour;
			_dateAdded = dateAdded;
			_streets = new SortedSet<Street>();

			if (streets != null)
				foreach (Street street in streets)
					if (street != null)
						_streets.Add(street);

			Assignee = null;
		}
		public DeliveryZone(string name, Colour colour, DateTime dateAdded, params Street[] streets)
			: this(name, colour, dateAdded, (IEnumerable<Street>)streets)
		{
		}
		public DeliveryZone(string name, Colour colour, DateTime dateAdded)
			: this(name, colour, dateAdded, null)
		{
		}

		public override string ToString()
		{
			return string.Format("{0}: {{{1}}}", _name, string.Join(", ", _streets));
		}
		public string Name
		{
			get
			{
				return _name;
			}
		}
		public Colour Colour
		{
			get
			{
				return _colour;
			}
		}
		public DateTime DateAdded
		{
			get
			{
				return _dateAdded;
			}
		}
		public ICollection<Street> Streets
		{
			get
			{
				return _streets;
			}
		}
		public Employee Assignee
		{
			get;
			set;
		}
		public static IEqualityComparer<DeliveryZone> IdentityComparer
		{
			get
			{
				return _identityComparer;
			}
		}

		private readonly string _name;
		private readonly Colour _colour;
		private readonly DateTime _dateAdded;
		private readonly ICollection<Street> _streets;
		private readonly static IEqualityComparer<DeliveryZone> _identityComparer = new DeliveryZoneIdentityComparer();

		private sealed class DeliveryZoneIdentityComparer
			: IEqualityComparer<DeliveryZone>
		{
			#region IEqualityComparer<DeliveryZone> Members
			public bool Equals(DeliveryZone one, DeliveryZone another)
			{
				if (one == null)
					return (another == null);
				else
					return (another != null
							&& string.Equals(one._name, another._name, StringComparison.OrdinalIgnoreCase));
			}
			public int GetHashCode(DeliveryZone value)
			{
				if (value == null)
					throw new ArgumentNullException("value");

				return value._name.GetHashCode();
			}
			#endregion
		}
	}
}