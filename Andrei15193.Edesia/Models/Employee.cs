using System;
namespace Andrei15193.Edesia.Models
{
	public class Employee
		: ApplicationUserRole
	{
		public Employee(ApplicationUser employee, double transportCapacity)
			: base(employee)
		{
			if (transportCapacity <= 0)
				throw new ArgumentException("Must be strictly positive", "transportCapacity");

			_transportCapacity = transportCapacity;
		}

		public double TransportCapacity
		{
			get
			{
				return _transportCapacity;
			}
			set
			{
				if (value < 1)
					throw new ArgumentException("Must be strictly positive", "TransportCapacity");

				_transportCapacity = value;
			}
		}

		private double _transportCapacity;
	}
}