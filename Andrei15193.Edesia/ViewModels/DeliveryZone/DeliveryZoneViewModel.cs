using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.DeliveryZone
{
	public class DeliveryZoneViewModel
	{
		static DeliveryZoneViewModel()
		{
			IDictionary<string, Colour> allColours = new SortedList<string, Colour>(capacity: 43);

			allColours.Add(ColourStrings.Amber, Colours.Amber);
			allColours.Add(ColourStrings.Blue, Colours.Blue);
			allColours.Add(ColourStrings.Brown, Colours.Brown);
			allColours.Add(ColourStrings.Cobalt, Colours.Cobalt);
			allColours.Add(ColourStrings.Crimson, Colours.Crimson);
			allColours.Add(ColourStrings.Cyan, Colours.Cyan);
			allColours.Add(ColourStrings.DarkBlue, Colours.DarkBlue);
			allColours.Add(ColourStrings.DarkBrown, Colours.DarkBrown);
			allColours.Add(ColourStrings.DarkCobalt, Colours.DarkCobalt);
			allColours.Add(ColourStrings.DarkCrimson, Colours.DarkCrimson);
			allColours.Add(ColourStrings.DarkCyan, Colours.DarkCyan);
			allColours.Add(ColourStrings.DarkEmerald, Colours.DarkEmerald);
			allColours.Add(ColourStrings.DarkGreen, Colours.DarkGreen);
			allColours.Add(ColourStrings.DarkIndigo, Colours.DarkIndigo);
			allColours.Add(ColourStrings.DarkMagenta, Colours.DarkMagenta);
			allColours.Add(ColourStrings.DarkOrange, Colours.DarkOrange);
			allColours.Add(ColourStrings.DarkPink, Colours.DarkPink);
			allColours.Add(ColourStrings.DarkRed, Colours.DarkRed);
			allColours.Add(ColourStrings.DarkTeal, Colours.DarkTeal);
			allColours.Add(ColourStrings.DarkViolet, Colours.DarkViolet);
			allColours.Add(ColourStrings.Emerald, Colours.Emerald);
			allColours.Add(ColourStrings.Green, Colours.Green);
			allColours.Add(ColourStrings.Indigo, Colours.Indigo);
			allColours.Add(ColourStrings.LightBlue, Colours.LightBlue);
			allColours.Add(ColourStrings.LighterBlue, Colours.LighterBlue);
			allColours.Add(ColourStrings.LightGreen, Colours.LightGreen);
			allColours.Add(ColourStrings.LightOlive, Colours.LightOlive);
			allColours.Add(ColourStrings.LightOrange, Colours.LightOrange);
			allColours.Add(ColourStrings.LightPink, Colours.LightPink);
			allColours.Add(ColourStrings.LightRed, Colours.LightRed);
			allColours.Add(ColourStrings.LightTeal, Colours.LightTeal);
			allColours.Add(ColourStrings.Lime, Colours.Lime);
			allColours.Add(ColourStrings.Magenta, Colours.Magenta);
			allColours.Add(ColourStrings.Mauve, Colours.Mauve);
			allColours.Add(ColourStrings.Olive, Colours.Olive);
			allColours.Add(ColourStrings.Orange, Colours.Orange);
			allColours.Add(ColourStrings.Pink, Colours.Pink);
			allColours.Add(ColourStrings.Red, Colours.Red);
			allColours.Add(ColourStrings.Steel, Colours.Steel);
			allColours.Add(ColourStrings.Taupe, Colours.Taupe);
			allColours.Add(ColourStrings.Teal, Colours.Teal);
			allColours.Add(ColourStrings.Violet, Colours.Violet);
			allColours.Add(ColourStrings.Yellow, Colours.Yellow);

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

		[LocalizedRequired(DeliveryZoneDetailsViewKey.DeliveryZoneTextBox_MissingValue, typeof(DeliveryZoneControllerStrings), AllowEmptyStrings = false)]
		[Display(Name = DeliveryZoneDetailsViewKey.DeliveryZoneTextBox_DisplayName, Prompt = DeliveryZoneDetailsViewKey.DeliveryZoneTextBox_Hint, ResourceType = typeof(DeliveryZoneControllerStrings))]
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

		[LocalizedRegularExpression("#([0-90a-fA-F]{6})", DeliveryZoneDetailsViewKey.DeliveryZoneColourTextBox_InvalidValue, typeof(DeliveryZoneControllerStrings))]
		[Display(Name = DeliveryZoneDetailsViewKey.DeliveryZoneColourComboBox_DisplayName, Prompt = DeliveryZoneDetailsViewKey.DeliveryZoneColourComboBox_Hint, ResourceType = typeof(DeliveryZoneControllerStrings))]
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

		[Display(Name = DeliveryZoneDetailsViewKey.AvailableAddressesListBox_DisplayName, Prompt = DeliveryZoneDetailsViewKey.AvailableAddressesListBox_Hint, ResourceType = typeof(DeliveryZoneControllerStrings))]
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

		[Display(Name = DeliveryZoneDetailsViewKey.EmployeesComobBox_DisplayName, Prompt = DeliveryZoneDetailsViewKey.EmployeesComobBox_Hint, ResourceType = typeof(DeliveryZoneControllerStrings))]
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