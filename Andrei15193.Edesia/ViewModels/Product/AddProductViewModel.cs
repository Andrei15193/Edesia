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
				yield return new ValidationResult(AddProductViewStrings.ProductCapacityTextBox_InvalidValue, new[] { "Capacity" });

			double price;
			if (!double.TryParse(Price, out price) || price < 0)
				yield return new ValidationResult(AddProductViewStrings.ProductPriceTextBox_InvalidValue, new[] { "Price" });

			if (!Regex.IsMatch(Image.FileName, @"\.(jpg|jpeg|gif|png)$", RegexOptions.IgnoreCase))
				yield return new ValidationResult(AddProductViewStrings.ProductImageTextBox_InvalidValue, new[] { "Image" });
		}
		#endregion

		[LocalizedRequired(AddProductViewKey.ProductNameTextBox_MissingValue, AllowEmptyStrings = false)]
		[Display(Name = AddProductViewKey.ProductNameTextBox_DisplayName, Prompt = AddProductViewKey.ProductNameTextBox_Hint, ResourceType = typeof(AddProductViewStrings))]
		public string Name
		{
			get;
			set;
		}

		[LocalizedRequired(AddProductViewKey.ProductPriceTextBox_MissingValue)]
		[Display(Name = AddProductViewKey.ProductPriceTextBox_DisplayName, Prompt = AddProductViewKey.ProductPriceTextBox_Hint, ResourceType = typeof(AddProductViewStrings))]
		public string Price
		{
			get;
			set;
		}

		[LocalizedRequired(AddProductViewKey.ProductCapacityTextBox_MissingValue)]
		[Display(Name = AddProductViewKey.ProductCapacityTextBox_DisplayName, Prompt = AddProductViewKey.ProductCapacityTextBox_Hint, ResourceType = typeof(AddProductViewStrings))]
		public string Capacity
		{
			get;
			set;
		}

		[LocalizedRequired(AddProductViewKey.ProductImageTextBox_MissingValue)]
		[Display(Name = AddProductViewKey.ProductImageTextBox_DisplayName, Prompt = AddProductViewKey.ProductImageTextBox_Hint, ResourceType = typeof(AddProductViewStrings))]
		public HttpPostedFileBase Image
		{
			get;
			set;
		}
	}
}