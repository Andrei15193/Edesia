using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.Delivery
{
	public class RemoveStreetViewModel
	{
		[Display(Name = RemoveStreetViewKey.StreetToRemoveComboBox_DisplayName, Prompt = RemoveStreetViewKey.StreetToRemoveComboBox_Hint, ResourceType = typeof(RemoveStreetViewStrings))]
		public string StreetToRemove
		{
			get;
			set;
		}
		public ISet<string> UnusedStreets
		{
			get
			{
				return _unusedStreets;
			}
		}

		private readonly ISet<string> _unusedStreets = new SortedSet<string>();
	}
}