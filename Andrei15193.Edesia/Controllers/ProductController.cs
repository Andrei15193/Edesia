using System;
using System.Web.Mvc;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.ViewModels.Product;
namespace Andrei15193.Edesia.Controllers
{
	[ConfirmAccess(typeof(Administrator))]
	public class ProductController
		: ApplicationController
	{
		[AllowAnonymous]
		public ActionResult Default()
		{
			ApplicationUserRole applicationUserRole = GetApplicationUser(HttpContext) as ApplicationUserRole;
			return View(new ProductsViewModel(_productRepository.GetProducts(), applicationUserRole != null && applicationUserRole.IsInRole<Administrator>()));
		}

		[HttpGet]
		public ActionResult Add()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Add(AddProductViewModel addProductViewModel)
		{
			if (ModelState.IsValid)
				try
				{
					Uri imageLocation;
					_imageUploader.Upload(addProductViewModel.Image.InputStream,
										  addProductViewModel.Image.FileName.Substring(Math.Max(addProductViewModel.Image.FileName.LastIndexOf('\\'), addProductViewModel.Image.FileName.LastIndexOf('/')) + 1),
										  out imageLocation);

					_productRepository.AddProduct(new Product(addProductViewModel.Name, double.Parse(addProductViewModel.Price), int.Parse(addProductViewModel.Capacity), imageLocation));
					return RedirectToAction("Default", "Product");
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueProductException uniqueProductException = aggregatedException as UniqueProductException;

						if (uniqueProductException != null)
							ModelState.AddModelError("Name", string.Format(ErrorStrings.ProductNameTextBox_DuplicateValue_Format, uniqueProductException.ConflictingValue));
					}
				}

			return View(addProductViewModel);
		}

		[HttpGet]
		public ActionResult Remove(string productName)
		{
			if (productName != null)
				_productRepository.RemoveProduct(productName);

			return RedirectToAction("Default", "Product");
		}

		private readonly IProductRepository _productRepository = (IProductRepository)MvcApplication.DependencyContainer["productRepository"];
		private readonly IImageUploader _imageUploader = (IImageUploader)MvcApplication.DependencyContainer["imageUploader"];
	}
}