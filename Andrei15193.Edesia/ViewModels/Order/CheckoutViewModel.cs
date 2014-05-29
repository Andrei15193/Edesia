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

		[LocalizedRequired(OrderControllerKey.SelectedStreetListBox_NoSelectedItem, typeof(OrderControllerStrings), AllowEmptyStrings = false)]
		[Display(Name = OrderControllerKey.SelectedStreetListBox_DisplayName, ResourceType = typeof(OrderControllerStrings))]
		public string SelectedStreet
		{
			get;
			set;
		}

		public IEnumerable<string> Streets
		{
			get
			{
				return _streets;
			}
			set
			{
				_streets = (value ?? MvcApplication.GetEmptyAray<string>());
			}
		}

		[LocalizedRequired(OrderControllerKey.AddressDetailsTextBox_MissingValue, typeof(OrderControllerStrings), AllowEmptyStrings = false)]
		[Display(Name = OrderControllerKey.AddressDetailsTextBox_DisplayName, Prompt = OrderControllerKey.AddressDetailsTextBox_Hint, ResourceType = typeof(OrderControllerStrings))]
		public string AddressDetails
		{
			get;
			set;
		}

		private IEnumerable<string> _streets;
	}
}