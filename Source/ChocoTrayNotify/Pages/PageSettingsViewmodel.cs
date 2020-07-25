using MSHC.WPF.MVVM;
using System;
using System.Windows.Input;

namespace ChocoTrayNotify.Pages
{
    public class PageSettingsViewmodel : ObservableObject
    {
        private CTNSettings _settings = GAS.Settings.Clone();
        public CTNSettings Settings
        {
            get { return _settings; }
            set { _settings = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand    => new RelayCommand(() => Save());

        public ICommand DiscardCommand => new RelayCommand(() => Discard());

        private void Save()
        {
            GAS.Settings.Apply(Settings);
            GAS.Settings.Save();
        }

        private void Discard()
        {
            Settings = GAS.Settings.Clone();
        }
    }
}
