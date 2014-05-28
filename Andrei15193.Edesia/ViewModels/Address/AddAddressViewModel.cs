using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.Address
{
	public class AddAddressViewModel
	{
		[LocalizedRequired(AddressControllerKey.AddressTextBox_MissingValue, typeof(AddressControllerStrings), AllowEmptyStrings = false)]
		[Display(Name = AddressControllerKey.AddressTextBox_DisplayName, Prompt = AddressControllerKey.AddressTextBox_Hint, ResourceType = typeof(AddressControllerStrings))]
		public string Address
		{
			get;
			set;
		}
	}
}