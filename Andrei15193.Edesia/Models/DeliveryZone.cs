using System;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.Models
{
	public class DeliveryZone
	{
		public DeliveryZone(string name, Colour colour, IEnumerable<string> streets)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Cannot be empty or whitespace!", "name");

			if (streets == null)
				throw new ArgumentNullException("streets");
			if (streets.Any(street => street == null || string.IsNullOrEmpty(street) || string.IsNullOrWhiteSpace(street)))
				throw new ArgumentException("Cannot contain null, empty or whitespace!", "streets");

			_name = name;
			_colour = colour;
			_streets = streets;
		}
		public DeliveryZone(string name, Colour colour, params string[] streets)
			: this(name, colour, (IEnumerable<string>)streets)
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
		public IEnumerable<string> Streets
		{
			get
			{
				return _streets;
			}
		}

		private readonly string _name;
		private readonly Colour _colour;
		private readonly IEnumerable<string> _streets;
	}
}