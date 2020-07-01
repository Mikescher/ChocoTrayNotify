using MSHC.WPF.MVVM;
using System;
using System.Windows.Media;

namespace ChocoTrayNotify.Converter
{
    class AppStateToIcon : OneWayConverter<AppStatus, ImageSource>
    {
        protected override ImageSource Convert(AppStatus value, object parameter)
        {
            var converter = new ImageSourceConverter();

            return value switch
            {
                AppStatus.Okay             => (ImageSource)converter.ConvertFromString("pack://application:,,,/icons/icon_off.ico"),
                AppStatus.UpdatesAvailable => (ImageSource)converter.ConvertFromString("pack://application:,,,/icons/icon_on.ico"),
                AppStatus.Refreshing       => (ImageSource)converter.ConvertFromString("pack://application:,,,/icons/icon_active.ico"),
                AppStatus.Error            => (ImageSource)converter.ConvertFromString("pack://application:,,,/icons/icon_error.ico"),

                _ => throw new ArgumentException(),
            };

        }
    }
}
