using System;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.Models
{
	public class DeliveryZone
	{
		public DeliveryZone(string name, Colour colour, IEnumerable<string> addresses)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Cannot be empty or whitespace!", "name");

			if (addresses == null)
				throw new ArgumentNullException("addresses");
			if (addresses.Any(address => address == null || string.IsNullOrEmpty(address) || string.IsNullOrWhiteSpace(address)))
				throw new ArgumentException("Cannot contain null, empty or whitespace!", "addresses");

			_name = name;
			_colour = colour;
			_addresses = new SortedSet<string>(addresses, StringComparer.Ordinal);
			Assignee = null;
		}
		public DeliveryZone(string name, Colour colour, params string[] addresses)
			: this(name, colour, (IEnumerable<string>)addresses)
		{
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
		public IEnumerable<string> Addresses
		{
			get
			{
				return _addresses;
			}
		}
		public Employee Assignee
		{
			get;
			set;
		}

		private readonly string _name;
		private readonly Colour _colour;
		private readonly IEnumerable<string> _addresses;
	}
}