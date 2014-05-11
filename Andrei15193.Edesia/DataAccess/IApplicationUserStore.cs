using System.Collections.Generic;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IApplicationUserStore
	{
		void AddApplicationUser(ApplicationUser applicationUser, string password, string registrationKey);
		ApplicationUser Find(string email, string authenticationToken, AuthenticationTokenType authenticationTokenType = AuthenticationTokenType.Password);
		void SetAuthenticationToken(ApplicationUser applicationUser, string authenticationToken, AuthenticationTokenType authenticationMethod = AuthenticationTokenType.Password);
		void ClearAuthenticationKey(string applicationUserEmail);
		bool ClearRegistrationKey(string applicationUserEmail, string applicationUserRegistrationKey);
		IEnumerable<DetailedAddress> GetAddresses();
	}
}