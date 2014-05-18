using System;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IProductProvider
	{
		Product GetProduct(string name, DateTime version);
		Product GetProduct(string name);
	}
}