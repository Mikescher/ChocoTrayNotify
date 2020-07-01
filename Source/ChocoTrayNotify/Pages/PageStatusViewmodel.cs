using ChocoTrayNotify.Choco;
using MSHC.WPF.MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChocoTrayNotify.Pages
{
    public class PageStatusViewmodel: ObservableObject
    {
        public ICommand RefreshCommand => new RelayCommand(() => App.CurrentApp.ChocoRefresh(true));

        public ObservableCollection<ChocoPackage> Packages { get; set; } = new ObservableCollection<ChocoPackage>();

        public GlobalAppState GAS => GlobalAppState.Inst;

        public PageStatusViewmodel()
        {
            foreach (var p in GAS.LastRefreshResult) Packages.Add(p);
        }
    }
}
