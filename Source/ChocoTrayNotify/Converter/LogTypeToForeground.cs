using ChocoTrayNotify.Log;
using MSHC.WPF.MVVM;
using System;
using System.Windows.Media;

namespace ChocoTrayNotify.Converter
{
    class LogTypeToForeground : OneWayConverter<CTLogType, Brush>
    {
        protected override Brush Convert(CTLogType value, object parameter)
        {
            return value switch
            {
                CTLogType.Debug      => Brushes.LightGray,
                CTLogType.Info       => Brushes.LightBlue,
                CTLogType.Error      => Brushes.Red,
                CTLogType.Command    => Brushes.Black,
                CTLogType.WebRequest => Brushes.Black,

                _                    => throw new ArgumentException(),
            };

        }
    }
}
