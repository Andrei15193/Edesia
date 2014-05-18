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

			allColours.Add(ColourNamesStrings.Amber, Colours.Amber);
			allColours.Add(ColourNamesStrings.Blue, Colours.Blue);
			allColours.Add(ColourNamesStrings.Brown, Colours.Brown);
			allColours.Add(ColourNamesStrings.Cobalt, Colours.Cobalt);
			allColours.Add(ColourNamesStrings.Crimson, Colours.Crimson);
			allColours.Add(ColourNamesStrings.Cyan, Colours.Cyan);
			allColours.Add(ColourNamesStrings.DarkBlue, Colours.DarkBlue);
			allColours.Add(ColourNamesStrings.DarkBrown, Colours.DarkBrown);
			allColours.Add(ColourNamesStrings.DarkCobalt, Colours.DarkCobalt);
			allColours.Add(ColourNamesStrings.DarkCrimson, Colours.DarkCrimson);
			allColours.Add(ColourNamesStrings.DarkCyan, Colours.DarkCyan);
			allColours.Add(ColourNamesStrings.DarkEmerald, Colours.DarkEmerald);
			allColours.Add(ColourNamesStrings.DarkGreen, Colours.DarkGreen);
			allColours.Add(ColourNamesStrings.DarkIndigo, Colours.DarkIndigo);
			allColours.Add(ColourNamesStrings.DarkMagenta, Colours.DarkMagenta);
			allColours.Add(ColourNamesStrings.DarkOrange, Colours.DarkOrange);
			allColours.Add(ColourNamesStrings.DarkPink, Colours.DarkPink);
			allColours.Add(ColourNamesStrings.DarkRed, Colours.DarkRed);
			allColours.Add(ColourNamesStrings.DarkTeal, Colours.DarkTeal);
			allColours.Add(ColourNamesStrings.DarkViolet, Colours.DarkViolet);
			allColours.Add(ColourNamesStrings.Emerald, Colours.Emerald);
			allColours.Add(ColourNamesStrings.Green, Colours.Green);
			allColours.Add(ColourNamesStrings.Indigo, Colours.Indigo);
			allColours.Add(ColourNamesStrings.LightBlue, Colours.LightBlue);
			allColours.Add(ColourNamesStrings.LighterBlue, Colours.LighterBlue);
			allColours.Add(ColourNamesStrings.LightGreen, Colours.LightGreen);
			allColours.Add(ColourNamesStrings.LightOlive, Colours.LightOlive);
			allColours.Add(ColourNamesStrings.LightOrange, Colours.LightOrange);
			allColours.Add(ColourNamesStrings.LightPink, Colours.LightPink);
			allColours.Add(ColourNamesStrings.LightRed, Colours.LightRed);
			allColours.Add(ColourNamesStrings.LightTeal, Colours.LightTeal);
			allColours.Add(ColourNamesStrings.Lime, Colours.Lime);
			allColours.Add(ColourNamesStrings.Magenta, Colours.Magenta);
			allColours.Add(ColourNamesStrings.Mauve, Colours.Mauve);
			allColours.Add(ColourNamesStrings.Olive, Colours.Olive);
			allColours.Add(ColourNamesStrings.Orange, Colours.Orange);
			allColours.Add(ColourNamesStrings.Pink, Colours.Pink);
			allColours.Add(ColourNamesStrings.Red, Colours.Red);
			allColours.Add(ColourNamesStrings.Steel, Colours.Steel);
			allColours.Add(ColourNamesStrings.Taupe, Colours.Taupe);
			allColours.Add(ColourNamesStrings.Teal, Colours.Teal);
			allColours.Add(ColourNamesStrings.Violet, Colours.Violet);
			allColours.Add(ColourNamesStrings.Yellow, Colours.Yellow);

			_availableColours = new ReadOnlyDictionary<string, Colour>(allColours);
		}

		public DeliveryZoneViewModel(IEnumerable<KeyValuePair<string, bool>> availableAddresses, IEnumerable<Employee> employees)
		{
			if (employees == null)
				_employees = new SortedSet<Employee>(Comparer<Employee>.Create((first, second) => first.LastName.CompareTo(second.LastName)));
			else
				_employees = new SortedSet<Employee>(employees, Comparer<Employee>.Create((first, second) => first.LastName.CompareTo(second.LastName)));

			if (availableAddresses == null)
				_availableAddresses = new SortedSet<KeyValuePair<string, bool>>(new KeyComparer());
			else
				_availableAddresses = new SortedSet<KeyValuePair<string, bool>>(availableAddresses.Where(availableAddress => !string.IsNullOrEmpty(availableAddress.Key) && !string.IsNullOrWhiteSpace(availableAddress.Key)), new KeyComparer());
		}
		public DeliveryZoneViewModel()
			: this(null, null)
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

		[Display(Name = DeliveryZoneDetailsViewKey.EmployeesComobBox_DisplayName, Prompt = DeliveryZoneDetailsViewKey.EmployeesComobBox_Hint, ResourceType = typeof(DeliveryZoneDetailsViewStrings))]
		public ISet<Employee> Employees
		{
			get
			{
				return _employees;
			}
		}
		public string SelectedEmployeeEMailAddress
		{
			get;
			set;
		}

		private readonly ISet<KeyValuePair<string, bool>> _availableAddresses;
		private readonly ISet<Employee> _employees;
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