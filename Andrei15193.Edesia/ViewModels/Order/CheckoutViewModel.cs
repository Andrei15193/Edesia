using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.Order
{
	public class CheckoutViewModel
	{
		public ShoppingCart ShoppingCart
		{
			get;
			set;
		}

		[LocalizedRequired(OrderControllerKey.SelectedAddressListBox_NoSelectedItem, typeof(OrderControllerStrings), AllowEmptyStrings = false)]
		[Display(Name = OrderControllerKey.SelectedAddressListBox_DisplayName, ResourceType = typeof(OrderControllerStrings))]
		public string SelectedAddress
		{
			get;
			set;
		}

		public IEnumerable<string> Addresses
		{
			get
			{
				return _addresses;
			}
			set
			{
				_addresses = (value ?? MvcApplication.GetEmptyAray<string>());
			}
		}

		[Display(Name = OrderControllerKey.AddressDetailsTextBox_DisplayName, Prompt = OrderControllerKey.AddressDetailsTextBox_Hint, ResourceType = typeof(OrderControllerStrings))]
		public string AddressDetails
		{
			get;
			set;
		}

		private IEnumerable<string> _addresses;
	}
}