using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.Delivery
{
	public class AddStreetViewModel
	{
		[LocalizedRequired(ErrorKey.StreetTextBox_MissingValue, AllowEmptyStrings = false)]
		[Display(Name = AddStreetViewKey.StreetNameTextBox_DisplayName, Prompt = AddStreetViewKey.StreetNameTextBox_Hint, ResourceType = typeof(AddStreetViewStrings))]
		public string StreetName
		{
			get
			{
				return _streetName;
			}
			set
			{
				if (value == null)
					_streetName = value;
				else
					_streetName = value.Trim();
			}
		}

		private string _streetName;
	}
}