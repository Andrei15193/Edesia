using System;
using System.Collections.Generic;
using System.Net.Mail;
namespace Andrei15193.Edesia.Models
{
	public class ApplicationUser
	{
		public ApplicationUser(string eMailAddress, string firstName, string lastName, DateTime registrationTime, Address address = null)
		{
			if (eMailAddress == null)
				throw new ArgumentNullException("eMailAddress");
			try
			{
				_eMailAddress = new MailAddress(eMailAddress).Address;
			}
			catch (FormatException formatException)
			{
				throw new ArgumentException("The given e-mail address is not valid!", "eMailAddress", formatException);
			}

			if (firstName == null)
				throw new ArgumentNullException("firstName");
			if (string.IsNullOrEmpty(firstName) || string.IsNullOrWhiteSpace(firstName))
				throw new ArgumentException("Cannot be empty or whitesapce!", "firstName");

			if (lastName == null)
				throw new ArgumentNullException("lastName");
			if (string.IsNullOrEmpty(lastName) || string.IsNullOrWhiteSpace(lastName))
				throw new ArgumentException("Cannot be empty or whitesapce!", "lastName");

			_firstName = firstName.Trim();
			_lastName = lastName.Trim();
			_registrationTime = registrationTime;

			Address = address;
		}
		protected ApplicationUser(ApplicationUser applicationUser)
		{
			if (applicationUser == null)
				throw new ArgumentNullException("applicationUser");

			_eMailAddress = applicationUser._eMailAddress;
			_firstName = applicationUser._firstName;
			_lastName = applicationUser._lastName;
			_registrationTime = applicationUser._registrationTime;
		}

		public string EMailAddress
		{
			get
			{
				return _eMailAddress;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("EMailAddress");
				try
				{
					_eMailAddress = new MailAddress(value).Address;
				}
				catch (FormatException formatException)
				{
					throw new ArgumentException("The given e-mail address is not valid!", "EMailAddress", formatException);
				}
			}
		}
		public string FirstName
		{
			get
			{
				return _firstName;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("FirstName");
				if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Cannot be empty or whitesapce!", "FirstName");

				_firstName = value.Trim();
			}
		}
		public string LastName
		{
			get
			{
				return _lastName;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("LastName");
				if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Cannot be empty or whitesapce!", "LastName");

				_lastName = value.Trim();
			}
		}
		public DateTime RegistrationTime
		{
			get
			{
				return _registrationTime;
			}
		}
		public Address Address
		{
			get;
			set;
		}
		public virtual IReadOnlyCollection<string> Roles
		{
			get
			{
				return MvcApplication.GetEmptyAray<string>();
			}
		}

		private string _eMailAddress;
		private string _firstName;
		private string _lastName;
		private readonly DateTime _registrationTime;
	}
}