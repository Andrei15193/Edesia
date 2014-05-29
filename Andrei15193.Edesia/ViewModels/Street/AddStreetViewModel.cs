using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.Street
{
	public class AddStreetViewModel
	{
		[LocalizedRequired(StreetControllerKey.StreetTextBox_MissingValue, typeof(StreetControllerStrings), AllowEmptyStrings = false)]
		[Display(Name = StreetControllerKey.StreetTextBox_DisplayName, Prompt = StreetControllerKey.StreetTextBox_Hint, ResourceType = typeof(StreetControllerStrings))]
		public string Street
		{
			get;
			set;
		}
	}
}