using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Andrei15193.Edesia.DataAccess;
namespace Andrei15193.Edesia.Models
{
	public interface IApplication
	{
		ApplicationUser AuthenticatedUser
		{
			get;
		}
		IApplicationUserStore ApplicationUserStore
		{
			get;
		}
	}
}