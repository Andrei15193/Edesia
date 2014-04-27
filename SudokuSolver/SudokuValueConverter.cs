using System;
using System.Windows.Data;
namespace SudokuSolver
{
	[ValueConversion(typeof(int), typeof(string))]
	internal class SudokuValueConverter
		: IValueConverter
	{
		#region IValueConverter Members
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null || value.Equals(0))
				return string.Empty;
			return value.ToString();
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int intValue;

			if (value == null || !int.TryParse(value.ToString(), out intValue))
				return 0;

			return value;
		}
		#endregion
	}
}