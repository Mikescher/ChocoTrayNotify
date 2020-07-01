﻿using ChocoTrayNotify.Choco;
using MSHC.WPF.MVVM;
using System;
using System.Windows.Media;

namespace ChocoTrayNotify.Converter
{
    class PackageToRowColor : OneWayConverter<ChocoPackage, Brush>
    {
        protected override Brush Convert(ChocoPackage value, object parameter)
        {
            if (parameter?.ToString() == "Background")
            {
                if (value.UpdateAvailable && value.Pinned != true) return Brushes.OrangeRed;

                if (value.UpdateAvailable) return Brushes.Yellow;

                return Brushes.White;
            }
            else if (parameter?.ToString() == "Foreground")
            {
                if (value.UpdateAvailable) return Brushes.Black;

                return Brushes.Gray;
            }

            throw new ArgumentException();

        }
    }
}
