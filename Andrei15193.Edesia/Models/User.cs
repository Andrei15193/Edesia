namespace Andrei15193.Edesia.Models
{
	public class User
	{
		public User(string username, string password, string email, string streetAddress)
		{
			Username = username;
			Password = password;
			Email = email;
			StreetAddress = streetAddress;
		}

		public int UserId
		{
			get;
			set;
		}
		public UserType UserType
		{
			get;
			set;
		}
		public string Username
		{
			get;
			set;
		}
		public string Password
		{
			get;
			set;
		}
		public string Email
		{
			get;
			set;
		}
		public string StreetAddress
		{
			get;
			set;
		}
	}
}