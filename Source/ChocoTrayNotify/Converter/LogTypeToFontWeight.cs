using ChocoTrayNotify.Log;
using MSHC.WPF.MVVM;
using System;
using System.Windows;

namespace ChocoTrayNotify.Converter
{
    class LogTypeToFontWeight : OneWayConverter<CTNLogType, FontWeight>
    {
        protected override FontWeight Convert(CTNLogType value, object parameter)
        {
            return value switch
            {
                CTNLogType.Debug      => FontWeights.Normal,
                CTNLogType.Info       => FontWeights.Normal,
                CTNLogType.Error      => FontWeights.Normal,
                CTNLogType.Command    => FontWeights.Bold,
                CTNLogType.WebRequest => FontWeights.Bold,

                _                    => throw new ArgumentException(),
            };

        }
    }
}
