using System;
using System.Collections.Generic;
namespace Andrei15193.Edesia.Models
{
	public class Employee
		: ApplicationUserRole
	{
		public Employee(ApplicationUser employee, int transportCapacity)
			: base(employee)
		{
			if (transportCapacity <= 0)
				throw new ArgumentException("Must be strictly positive", "transportCapacity");

			_transportCapacity = transportCapacity;
		}

		public int TransportCapacity
		{
			get
			{
				return _transportCapacity;
			}
			set
			{
				if (value <= 0)
					throw new ArgumentException("Must be strictly positive", "TransportCapacity");

				_transportCapacity = value;
			}
		}

		private int _transportCapacity;
		private readonly ISet<string> _deliveryZones = new SortedSet<string>();
	}
}