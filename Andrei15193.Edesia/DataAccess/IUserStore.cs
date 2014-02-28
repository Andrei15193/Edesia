using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IUserStore
	{
		void AddUser(User user, string password, string registrationKey);
		User Find(string email, string authenticationToken, AuthenticationTokenType authenticationTokenType = AuthenticationTokenType.Password);
		void SetAuthenticationToken(User user, string authenticationToken, AuthenticationTokenType authenticationMethod = AuthenticationTokenType.Password);
		void ClearAuthenticationKey(User user);
		bool ClearRegistrationKey(string userEmail, string userRegistrationKey);
	}
}