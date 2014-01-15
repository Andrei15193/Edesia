using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Andrei15193.Edesia.Models;

namespace Andrei15193.Edesia.Data
{
	public class MockData
	{
		public static readonly IReadOnlyList<User> Users = new[]
		{
			new User("Admin", "Admin", "admin@domain.tld", "Cireșilor 10"),
			new User("Client", "Client", "client@domain.tld", "Cireșilor 12"),
			new User("Angajat", "Angajat", "angajat@domain.tld", "Cireșilor 14"),
		};
		public static readonly IReadOnlyList<Product> Products = new[]
		{
			new Product("Lapte 1L", "Napolact")
			{
				ProductId = 0
			},
			new Product("Unt 200g", "Napolact")
			{
				ProductId = 1
			},
			new Product("Telemea cu gust domol 350g", "Napolact")
			{
				ProductId = 2
			}
		};
		public static readonly IReadOnlyList<Shop> Shops = new[]
		{
			new Shop("Billa", "Calea Floresti nr. 56, Cluj-Napoca, Cluj")
			{
				ShopId = 0
			},
			new Shop("Billa", "Regele Ferdinand nr. 22-26, Cluj-Napoca, Cluj")
			{
				ShopId = 1
			},
			new Shop("Cora", "B-dul 1 Decembrie 1918 nr. 142, Cluj-Napoca, Cluj")
			{
				ShopId = 2
			}
		};
		public static readonly IReadOnlyList<Offer> Offers = new[]
		{
			new Offer(Shops[0], Products[0], 300),
			new Offer(Shops[0], Products[1], 350),
			new Offer(Shops[0], Products[2], 200),
			new Offer(Shops[1], Products[0], 290),
			new Offer(Shops[1], Products[1], 370),
			new Offer(Shops[2], Products[0], 290),
			new Offer(Shops[2], Products[1], 390),
			new Offer(Shops[2], Products[2], 110)
		};
	}
}