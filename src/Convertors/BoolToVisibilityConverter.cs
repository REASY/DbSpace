using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DbSpace.Convertors
{
	[ValueConversion(typeof(object), typeof(Visibility))]
	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter,
		 System.Globalization.CultureInfo culture)
		{
			return System.Convert.ToBoolean(value) ? Visibility.Visible :
			(parameter != null && ((string)parameter) == "Hidden" ? Visibility.Hidden : Visibility.Collapsed);
		}

		public object ConvertBack(object value, Type targetType, object parameter,
		 System.Globalization.CultureInfo culture)
		{
			Visibility? visibleValue = value as Visibility?;
			if (visibleValue != null)
			{
				if (visibleValue.Value == Visibility.Visible)
					return true;
				else if (visibleValue.Value == Visibility.Collapsed || visibleValue.Value == Visibility.Hidden)
					return false;
			}
			return null;
		}
	}

}
