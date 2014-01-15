using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Andrei15193.Edesia.Data;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.Controllers
{
	public class ProductController
		: Controller
	{
		public ActionResult GetProducts()
		{
			return View(MockData.Offers
								.GroupBy(offer => offer.Product)
								.Select(grouping => new KeyValuePair<Product, int>(grouping.Key, grouping.Count()))
								.Where(productAndOffers => productAndOffers.Value > 0));
		}
		public ActionResult Show(string id)
		{
			int productId = int.Parse(id);
			return View(MockData.Offers.Where(offer => (offer.Product.ProductId == productId)));
		}
	}
}