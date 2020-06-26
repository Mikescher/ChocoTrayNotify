using ChocoTrayNotify.Choco;
using MSHC.WPF.MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChocoTrayNotify.Windows
{
    public class MainWindowViewmodel: ObservableObject
    {
        public ICommand RefreshCommand => new RelayCommand(Refresh);

        public ObservableCollection<ChocoPackage> Packages { get; set; } = new ObservableCollection<ChocoPackage>();

        private async void Refresh()
        {
            Packages.Clear();
            var chocoresult = await ChocoQueryExecutor.QueryFull();
            foreach (var pkg in chocoresult) Packages.Add(pkg);
        }
    }
}
