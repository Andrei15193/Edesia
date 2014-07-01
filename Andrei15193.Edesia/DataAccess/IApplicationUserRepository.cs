using System;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	[Obsolete]
	public interface IApplicationUserRepository
		: IApplicationUserProvider
	{
		void AddToCart(ApplicationUser applicationUser, OrderedProduct orderedProduct);
		void UpdateCart(ApplicationUser applicationUser, OrderedProduct orderedProduct);
		void RemoveFromCart(ApplicationUser applicationUser, Product product);
		void RemoveFromCarts(Product product);
		void ClearShoppingCart(ApplicationUser applicationUser);

		void EnrollAdministrator(string eMailAddress);
		void EnrollEmployee(string eMailAddress, double transportCapacity);

		void AddApplicationUser(ApplicationUser applicationUser, string password, string registrationKey);

		ApplicationUser Find(string eMail, string authenticationToken, AuthenticationTokenType authenticationTokenType = AuthenticationTokenType.Password);

		void SetAuthenticationToken(ApplicationUser applicationUser, string authenticationToken, AuthenticationTokenType authenticationMethod = AuthenticationTokenType.Password);
		void ClearAuthenticationKey(string applicationUserEmail);
		bool ClearRegistrationKey(string applicationUserEmail, string applicationUserRegistrationKey);
	}
}