using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.Product
{
	public class AddProductViewModel
	{
		[LocalizedRequired(ErrorKey.ProductNameTextBox_MissingValue, AllowEmptyStrings = false)]
		[Display(Name = AddProductViewKey.ProductNameTextBox_DisplayName, Prompt = AddProductViewKey.ProductNameTextBox_Hint, ResourceType = typeof(AddProductViewStrings))]
		public string Name
		{
			get;
			set;
		}
		[LocalizedRequired(ErrorKey.ProductPriceTextBox_MissingValue)]
		[Range(0, double.MaxValue, ErrorMessage = null, ErrorMessageResourceName = ErrorKey.ProductPriceTextBox_InvalidValue, ErrorMessageResourceType = typeof(ErrorStrings))]
		[Display(Name = AddProductViewKey.ProductPriceTextBox_DisplayName, Prompt = AddProductViewKey.ProductPriceTextBox_Hint, ResourceType = typeof(AddProductViewStrings))]
		public double Price
		{
			get;
			set;
		}
	}
}