using System;
using System.Collections.Generic;
namespace Andrei15193.Edesia.Models
{
	public class ApplicationUser
	{
		public ApplicationUser(string email, DateTime registrationTime, Address deliveryAddress = null)
		{
			_email = email;
			_registrationTime = registrationTime;
			_deliveryAddress = deliveryAddress;
		}

		public DateTime RegistrationTime
		{
			get
			{
				return _registrationTime;
			}
		}
		public string EMail
		{
			get
			{
				return _email;
			}
		}
		public Address DeliveryAddress
		{
			get;
			set;
		}
		public ISet<string> Roles
		{
			get
			{
				return _roles;
			}
		}

		private string _email;
		private Address _deliveryAddress;
		private readonly DateTime _registrationTime;
		private readonly ISet<string> _roles = new HashSet<string>();
	}
}