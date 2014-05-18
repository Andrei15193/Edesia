using System;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IApplicationUserProvider
	{
		ApplicationUser GetUser(string eMailAddress, DateTime version);
		ApplicationUser GetUser(string eMailAddress);
	}
}