using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.Product
{
	public class AddProductViewModel
		: IValidatableObject
	{
		#region IValidatableObject Members
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			int capacity;
			if (!int.TryParse(Capacity, out capacity) || capacity <= 0)
				yield return new ValidationResult(ProductControllerStrings.ProductCapacityTextBox_InvalidValue, new[] { "Capacity" });

			double price;
			if (!double.TryParse(Price, out price) || price < 0)
				yield return new ValidationResult(ProductControllerStrings.ProductPriceTextBox_InvalidValue, new[] { "Price" });

			if (!Regex.IsMatch(Image.FileName, @"\.(jpg|jpeg|gif|png)$", RegexOptions.IgnoreCase))
				yield return new ValidationResult(ProductControllerStrings.ProductImageTextBox_InvalidValue, new[] { "Image" });
		}
		#endregion

		[LocalizedRequired(AddProductViewKey.ProductNameTextBox_MissingValue, typeof(ProductControllerStrings), AllowEmptyStrings = false)]
		[Display(Name = AddProductViewKey.ProductNameTextBox_DisplayName, Prompt = AddProductViewKey.ProductNameTextBox_Hint, ResourceType = typeof(ProductControllerStrings))]
		public string Name
		{
			get;
			set;
		}

		[LocalizedRequired(AddProductViewKey.ProductPriceTextBox_MissingValue, typeof(ProductControllerStrings))]
		[Display(Name = AddProductViewKey.ProductPriceTextBox_DisplayName, Prompt = AddProductViewKey.ProductPriceTextBox_Hint, ResourceType = typeof(ProductControllerStrings))]
		public string Price
		{
			get;
			set;
		}

		[LocalizedRequired(AddProductViewKey.ProductCapacityTextBox_MissingValue, typeof(ProductControllerStrings))]
		[Display(Name = AddProductViewKey.ProductCapacityTextBox_DisplayName, Prompt = AddProductViewKey.ProductCapacityTextBox_Hint, ResourceType = typeof(ProductControllerStrings))]
		public string Capacity
		{
			get;
			set;
		}

		[LocalizedRequired(AddProductViewKey.ProductImageTextBox_MissingValue, typeof(ProductControllerStrings))]
		[Display(Name = AddProductViewKey.ProductImageTextBox_DisplayName, Prompt = AddProductViewKey.ProductImageTextBox_Hint, ResourceType = typeof(ProductControllerStrings))]
		public HttpPostedFileBase Image
		{
			get;
			set;
		}
	}
}