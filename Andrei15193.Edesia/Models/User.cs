using System;
using System.Collections.Generic;
namespace Andrei15193.Edesia.Models
{
	public class User
	{
		public User(string email, DateTime registrationTime)
		{
			_email = email;
			_registrationTime = registrationTime;
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
		public Address DefaultAddress
		{
			get;
			set;
		}
		public ICollection<Address> Addresses
		{
			get
			{
				return _addresses;
			}
		}
		public ISet<string> Roles
		{
			get
			{
				return _roles;
			}
		}

		private string _email;
		private readonly DateTime _registrationTime;
		private readonly ICollection<Address> _addresses = new LinkedList<Address>();
		private readonly ISet<string> _roles = new HashSet<string>();
	}
}