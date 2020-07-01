using ChocoTrayNotify.MVVM;
using System;
using System.Windows;

namespace ChocoTrayNotify.Converter
{
    class BoolToVisibility : OneWayConverter<bool, Visibility>
	{
		protected override Visibility Convert(bool value, object parameter)
		{
			if (string.IsNullOrWhiteSpace(parameter?.ToString()))
			{
				return value ? Visibility.Visible : Visibility.Hidden;
			}

			if (value)
				return (Visibility)Enum.Parse(typeof(Visibility), parameter.ToString().Split(';')[0]);
			return (Visibility)Enum.Parse(typeof(Visibility), parameter.ToString().Split(';')[1]);

		}
	}
}
