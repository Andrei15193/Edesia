using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.Address
{
	public class AddAddressViewModel
	{
		[LocalizedRequired(AddAddressViewKey.AddressTextBox_MissingValue, typeof(AddressControllerStrings), AllowEmptyStrings = false)]
		[Display(Name = AddAddressViewKey.AddressTextBox_DisplayName, Prompt = AddAddressViewKey.AddressTextBox_Hint, ResourceType = typeof(AddressControllerStrings))]
		public string Address
		{
			get;
			set;
		}
	}
}