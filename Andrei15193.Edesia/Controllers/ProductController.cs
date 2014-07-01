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
	public class ProductController
		: ApplicationController
	{
		[HttpGet]
		public ActionResult Default()
		{
			return View(_productRepository.GetProducts());
		}

		[HttpGet, Authorize, Role(typeof(Administrator))]
		public ActionResult Add()
		{
			return View();
		}
		[HttpPost, Authorize, Role(typeof(Administrator))]
		public ActionResult Add(AddProductViewModel addProductViewModel)
		{
			if (ModelState.IsValid)
				try
				{
					Uri imageLocation;
					_imageUploader.Upload(addProductViewModel.Image.InputStream,
										  addProductViewModel.Image.FileName.Substring(Math.Max(addProductViewModel.Image.FileName.LastIndexOf('\\'), addProductViewModel.Image.FileName.LastIndexOf('/')) + 1),
										  out imageLocation);

					_productRepository.Add(new Product(addProductViewModel.Name, double.Parse(addProductViewModel.Price), double.Parse(addProductViewModel.Capacity), imageLocation));
					return RedirectToAction("Default", "Product");
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueProductException uniqueProductException = aggregatedException as UniqueProductException;

						if (uniqueProductException != null)
							ModelState.AddModelError("Name", string.Format(ProductControllerStrings.ProductNameTextBox_DuplicateValue_Format, uniqueProductException.ConflictingValue));
					}
				}

			return View(addProductViewModel);
		}

		[HttpGet, Authorize, Role(typeof(Administrator))]
		public ActionResult Remove(string productName)
		{
			if (productName != null)
				_productRepository.Remove(productName);

			return RedirectToAction("Default", "Product");
		}

		private readonly IImageUploader _imageUploader = (IImageUploader)MvcApplication.DependencyContainer["imageUploader"];
		private readonly IProductRepository _productRepository = (IProductRepository)MvcApplication.DependencyContainer["productRepo"];
	}
}