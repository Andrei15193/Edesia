using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.Delivery
{
	public class DeliveryZoneViewModel
	{
		static DeliveryZoneViewModel()
		{
			IDictionary<string, Colour> allColours = new SortedList<string, Colour>(capacity: 43);

			allColours.Add("Amber", Colours.Amber);
			allColours.Add("Blue", Colours.Blue);
			allColours.Add("Brown", Colours.Brown);
			allColours.Add("Cobalt", Colours.Cobalt);
			allColours.Add("Crimson", Colours.Crimson);
			allColours.Add("Cyan", Colours.Cyan);
			allColours.Add("Dark blue", Colours.DarkBlue);
			allColours.Add("Dark brown", Colours.DarkBrown);
			allColours.Add("Dark cobalt", Colours.DarkCobalt);
			allColours.Add("Dark crimson", Colours.DarkCrimson);
			allColours.Add("Dark cyan", Colours.DarkCyan);
			allColours.Add("Dark emerald", Colours.DarkEmerald);
			allColours.Add("Dark green", Colours.DarkGreen);
			allColours.Add("Dark indigo", Colours.DarkIndigo);
			allColours.Add("Dark magenta", Colours.DarkMagenta);
			allColours.Add("Dark orange", Colours.DarkOrange);
			allColours.Add("Dark pink", Colours.DarkPink);
			allColours.Add("Dark red", Colours.DarkRed);
			allColours.Add("Dark teal", Colours.DarkTeal);
			allColours.Add("Dark violet", Colours.DarkViolet);
			allColours.Add("Emerald", Colours.Emerald);
			allColours.Add("Green", Colours.Green);
			allColours.Add("Indigo", Colours.Indigo);
			allColours.Add("Light blue", Colours.LightBlue);
			allColours.Add("Lighter blue", Colours.LighterBlue);
			allColours.Add("Light green", Colours.LightGreen);
			allColours.Add("Light olive", Colours.LightOlive);
			allColours.Add("Light orange", Colours.LightOrange);
			allColours.Add("Light pink", Colours.LightPink);
			allColours.Add("Light red", Colours.LightRed);
			allColours.Add("Light teal", Colours.LightTeal);
			allColours.Add("Lime", Colours.Lime);
			allColours.Add("Magenta", Colours.Magenta);
			allColours.Add("Mauve", Colours.Mauve);
			allColours.Add("Olive", Colours.Olive);
			allColours.Add("Orange", Colours.Orange);
			allColours.Add("Pink", Colours.Pink);
			allColours.Add("Red", Colours.Red);
			allColours.Add("Steel", Colours.Steel);
			allColours.Add("Taupe", Colours.Taupe);
			allColours.Add("Teal", Colours.Teal);
			allColours.Add("Violet", Colours.Violet);
			allColours.Add("Yellow", Colours.Yellow);

			_availableColours = new ReadOnlyDictionary<string, Colour>(allColours);
		}

		public DeliveryZoneViewModel(IEnumerable<KeyValuePair<string, bool>> availableAddresses)
		{
			if (availableAddresses == null)
				_availableAddresses = new SortedSet<KeyValuePair<string, bool>>(new KeyComparer());
			else
				_availableAddresses = new SortedSet<KeyValuePair<string, bool>>(availableAddresses.Where(availableAddress => !string.IsNullOrEmpty(availableAddress.Key) && !string.IsNullOrWhiteSpace(availableAddress.Key)), new KeyComparer());
		}
		public DeliveryZoneViewModel()
			: this(null)
		{
		}

		[LocalizedRequired(ErrorKey.DeliveryZoneTextBox_MissingValue, AllowEmptyStrings = false)]
		[Display(Name = DeliveryZoneDetailsViewKey.DeliveryZoneTextBox_DisplayName, Prompt = DeliveryZoneDetailsViewKey.DeliveryZoneTextBox_Hint, ResourceType = typeof(DeliveryZoneDetailsViewStrings))]
		public string DeliveryZoneName
		{
			get;
			set;
		}
		public string DeliveryZoneOldName
		{
			get;
			set;
		}

		[LocalizedRegularExpression("#([0-90a-fA-F]{6})", ErrorKey.DeliveryZoneColourTextBox_InvalidValue)]
		[Display(Name = DeliveryZoneDetailsViewKey.DeliveryZoneColourComboBox_DisplayName, Prompt = DeliveryZoneDetailsViewKey.DeliveryZoneColourComboBox_Hint, ResourceType = typeof(DeliveryZoneDetailsViewStrings))]
		public string DeliveryZoneColour
		{
			get;
			set;
		}
		public IReadOnlyDictionary<string, Colour> AvailableColours
		{
			get
			{
				return _availableColours;
			}
		}

		[Display(Name = DeliveryZoneDetailsViewKey.AvailableAddressesListBox_DisplayName, Prompt = DeliveryZoneDetailsViewKey.AvailableAddressesListBox_Hint, ResourceType = typeof(DeliveryZoneDetailsViewStrings))]
		public ISet<KeyValuePair<string, bool>> AvailableAddresses
		{
			get
			{
				return _availableAddresses;
			}
		}
		public string SubmitButtonText
		{
			get;
			set;
		}

		private ISet<KeyValuePair<string, bool>> _availableAddresses;
		private static IReadOnlyDictionary<string, Colour> _availableColours;

		private sealed class KeyComparer
			: IComparer<KeyValuePair<string, bool>>
		{
			#region IComparer<KeyValuePair<string,bool>> Members
			public int Compare(KeyValuePair<string, bool> x, KeyValuePair<string, bool> y)
			{
				return x.Key.CompareTo(y.Key);
			}
			#endregion
		}
	}
}