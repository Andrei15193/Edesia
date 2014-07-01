using System.Collections.Generic;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IUserRepository
	{
		IEnumerable<ApplicationUser> Users
		{
			get;
		}
		IEnumerable<Employee> GetEmployees();

		bool ConfirmUser(string eMail, string registrationToken);
		void Add(ApplicationUser user, string password, string registrationToken);
		void Update(ApplicationUser user);
		ApplicationUser Find(string eMail);
		ApplicationUser Find(string eMail, string authenticationToken, AuthenticationTokenType authenticationTokenType = AuthenticationTokenType.Password);
		void SetAuthenticationToken(ApplicationUser applicationUser, string authenticationToken, AuthenticationTokenType authenticationTokenType = AuthenticationTokenType.Password);

		void AddToShoppingCart(ApplicationUser owner, ShoppingCartEntry shoppingCartEntry);
		void UpdateShoppingCart(ApplicationUser owner, ShoppingCartEntry shoppingCartEntry);
		void RemoveFromShoppingCart(ApplicationUser owner, Product product);
		void Update(ShoppingCart shoppingCart);
		void ClearShoppingCart(ApplicationUser owner);
		ShoppingCart GetShoppingCart(ApplicationUser owner);
	}
}