using ChocoTrayNotify.Log;
using MSHC.WPF.MVVM;
using System;
using System.Windows.Media;

namespace ChocoTrayNotify.Converter
{
    class LogTypeToForeground : OneWayConverter<CTNLogType, Brush>
    {
        protected override Brush Convert(CTNLogType value, object parameter)
        {
            return value switch
            {
                CTNLogType.Debug      => Brushes.Gray,
                CTNLogType.Info       => Brushes.Black,
                CTNLogType.Error      => Brushes.Red,
                CTNLogType.Command    => Brushes.Black,
                CTNLogType.WebRequest => Brushes.Black,

                _                    => throw new ArgumentException(),
            };

        }
    }
}
