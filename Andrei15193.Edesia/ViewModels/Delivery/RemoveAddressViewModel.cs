using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.Delivery
{
	public class RemoveAddressViewModel
	{
		[Display(Name = RemoveAddressViewKey.AddressToRemoveComboBox_DisplayName, Prompt = RemoveAddressViewKey.AddressToRemoveComboBox_Hint, ResourceType = typeof(RemoveAddressViewStrings))]
		public string AddressToRemove
		{
			get;
			set;
		}
		public ISet<string> UnusedAddresses
		{
			get
			{
				return _unusedAddresses;
			}
		}

		private readonly ISet<string> _unusedAddresses = new SortedSet<string>();
	}
}