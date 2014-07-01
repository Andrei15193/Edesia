using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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

		public DeliveryZoneViewModel(IEnumerable<AvailableStreet> availableStreets, IEnumerable<Employee> employees)
		{
			_employees = new SortedSet<Employee>(EmployeeComparer.Instance);
			if (employees != null)
				foreach (Employee employee in employees)
					if (employee != null)
						_employees.Add(employee);

			_availableStreets = new SortedSet<AvailableStreet>(AvailableStreetComparer.Instance);
			if (availableStreets != null)
				foreach (AvailableStreet availableStreet in availableStreets)
					_availableStreets.Add(availableStreet);
		}
		public DeliveryZoneViewModel()
			: this(null, null)
		{
		}

		[LocalizedRequired(DeliveryZoneControllerKey.DeliveryZoneTextBox_MissingValue, typeof(DeliveryZoneControllerStrings), AllowEmptyStrings = false)]
		[Display(Name = DeliveryZoneControllerKey.DeliveryZoneTextBox_DisplayName, Prompt = DeliveryZoneControllerKey.DeliveryZoneTextBox_Hint, ResourceType = typeof(DeliveryZoneControllerStrings))]
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

		[LocalizedRegularExpression("#([0-90a-fA-F]{6})", DeliveryZoneControllerKey.DeliveryZoneColourTextBox_InvalidValue, typeof(DeliveryZoneControllerStrings))]
		[Display(Name = DeliveryZoneControllerKey.DeliveryZoneColourComboBox_DisplayName, Prompt = DeliveryZoneControllerKey.DeliveryZoneColourComboBox_Hint, ResourceType = typeof(DeliveryZoneControllerStrings))]
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

		[Display(Name = DeliveryZoneControllerKey.AvailableStreetsListBox_DisplayName, Prompt = DeliveryZoneControllerKey.AvailableStreetsListBox_Hint, ResourceType = typeof(DeliveryZoneControllerStrings))]
		public ISet<AvailableStreet> AvailableStreets
		{
			get
			{
				return _availableStreets;
			}
		}

		[Display(Name = DeliveryZoneControllerKey.EmployeesComobBox_DisplayName, Prompt = DeliveryZoneControllerKey.EmployeesComobBox_Hint, ResourceType = typeof(DeliveryZoneControllerStrings))]
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

		private readonly ISet<AvailableStreet> _availableStreets;
		private readonly ISet<Employee> _employees;
		private static IReadOnlyDictionary<string, Colour> _availableColours;

		private sealed class AvailableStreetComparer
			: IComparer<AvailableStreet>
		{
			private AvailableStreetComparer()
			{
			}

			#region IComparer<AvailableStreet> Members
			public int Compare(AvailableStreet first, AvailableStreet second)
			{
				return first.Name.CompareTo(second.Name);
			}
			#endregion

			public static IComparer<AvailableStreet> Instance
			{
				get
				{
					return _instace;
				}
			}

			private static readonly IComparer<AvailableStreet> _instace = new AvailableStreetComparer();
		}

		private sealed class EmployeeComparer
			: IComparer<Employee>
		{
			private EmployeeComparer()
			{
			}

			#region IComparer<Employee> Members
			public int Compare(Employee first, Employee second)
			{
				int firstNameCompareResult = first.FirstName.CompareTo(second.FirstName);

				if (firstNameCompareResult == 0)
					return first.LastName.CompareTo(second.LastName);
				else
					return firstNameCompareResult;
			}
			#endregion

			public static IComparer<Employee> Instance
			{
				get
				{
					return _instance;
				}
			}

			private static readonly IComparer<Employee> _instance = new EmployeeComparer();
		}
	}
}