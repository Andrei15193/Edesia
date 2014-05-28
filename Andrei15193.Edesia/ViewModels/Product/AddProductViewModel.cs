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

		[LocalizedRequired(ProductControllerKey.ProductNameTextBox_MissingValue, typeof(ProductControllerStrings), AllowEmptyStrings = false)]
		[Display(Name = ProductControllerKey.ProductNameTextBox_DisplayName, Prompt = ProductControllerKey.ProductNameTextBox_Hint, ResourceType = typeof(ProductControllerStrings))]
		public string Name
		{
			get;
			set;
		}

		[LocalizedRequired(ProductControllerKey.ProductPriceTextBox_MissingValue, typeof(ProductControllerStrings))]
		[Display(Name = ProductControllerKey.ProductPriceTextBox_DisplayName, Prompt = ProductControllerKey.ProductPriceTextBox_Hint, ResourceType = typeof(ProductControllerStrings))]
		public string Price
		{
			get;
			set;
		}

		[LocalizedRequired(ProductControllerKey.ProductCapacityTextBox_MissingValue, typeof(ProductControllerStrings))]
		[Display(Name = ProductControllerKey.ProductCapacityTextBox_DisplayName, Prompt = ProductControllerKey.ProductCapacityTextBox_Hint, ResourceType = typeof(ProductControllerStrings))]
		public string Capacity
		{
			get;
			set;
		}

		[LocalizedRequired(ProductControllerKey.ProductImageTextBox_MissingValue, typeof(ProductControllerStrings))]
		[Display(Name = ProductControllerKey.ProductImageTextBox_DisplayName, Prompt = ProductControllerKey.ProductImageTextBox_Hint, ResourceType = typeof(ProductControllerStrings))]
		public HttpPostedFileBase Image
		{
			get;
			set;
		}
	}
}