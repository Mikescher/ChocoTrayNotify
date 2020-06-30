using ChocoTrayNotify.MVVM;

namespace ChocoTrayNotify.Converter
{
    class BooleanNot : TwoWayConverter<bool, bool>
    {
        protected override bool Convert(bool value, object parameter) => !value;

        protected override bool ConvertBack(bool value, object parameter) => !value;
    }
}
