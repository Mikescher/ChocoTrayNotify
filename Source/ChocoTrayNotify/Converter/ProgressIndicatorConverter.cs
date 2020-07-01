using MSHC.WPF.MVVM;
using System.Windows.Media;

namespace ChocoTrayNotify.Converter
{
    class ProgressIndicatorConverter : OneWayConverter<int, Brush>
    {
        protected override Brush Convert(int value, object parameter)
		{
			int index;
			if (!int.TryParse((parameter ?? "").ToString(), out index)) return Brushes.Magenta;

			if (value < 0)
				return Brushes.Tomato;

			if (value <= index)
				return Brushes.WhiteSmoke;

			return new SolidColorBrush(Color.FromRgb(52, 152, 219));
		}
    }
}
