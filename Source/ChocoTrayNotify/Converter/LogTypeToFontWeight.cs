using ChocoTrayNotify.Log;
using MSHC.WPF.MVVM;
using System;
using System.Windows;

namespace ChocoTrayNotify.Converter
{
    class LogTypeToFontWeight : OneWayConverter<CTLogType, FontWeight>
    {
        protected override FontWeight Convert(CTLogType value, object parameter)
        {
            return value switch
            {
                CTLogType.Debug      => FontWeights.Normal,
                CTLogType.Info       => FontWeights.Normal,
                CTLogType.Error      => FontWeights.Normal,
                CTLogType.Command    => FontWeights.Bold,
                CTLogType.WebRequest => FontWeights.Bold,

                _                    => throw new ArgumentException(),
            };

        }
    }
}
