using ChocoTrayNotify.Util;
using MSHC.WPF.MVVM;

namespace ChocoTrayNotify.Converter
{
	public class PropertyToHelpText : OneWayConverter<string, string>
	{
		protected override string Convert(string value, object parameter)
		{
			return HelpTextsLoader.Get(value);
		}
	}
}
