using ChocoTrayNotify.Choco;
using ChocoTrayNotify.Log;
using MSHC.WPF.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ChocoTrayNotify.Windows
{
    public class MainWindowViewmodel: ObservableObject
    {
        public ICommand RefreshCommand => new RelayCommand(ManualRefresh);

        public ObservableCollection<ChocoPackage> Packages { get; set; } = new ObservableCollection<ChocoPackage>();

        public ObservableCollection<CTLogEntry> Logs { get; } = CTLog.Inst.Entries;

        private CTLogEntry _selectedLogItem = null;
        public  CTLogEntry SelectedLogItem
        {
            get { return _selectedLogItem; }
            set { _selectedLogItem = value; OnPropertyChanged(); }
        }

        private bool _isRefreshing = false;
        public  bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { _isRefreshing = value; OnPropertyChanged(); }
        }

        private async void ManualRefresh()
        {
            try
            {
                IsRefreshing = true;

                Packages.Clear();
                var chocoresult = await ChocoQueryExecutor.QueryFull();

                //chocoresult = chocoresult
                //    .OrderByDescending(p => p.UpdateAvailable && p.Pinned != true)
                //    .ThenBy(p => p.Pinned)
                //    .ThenBy(p => p.PackageName)
                //    .ToList();

                chocoresult = chocoresult
                    .OrderBy(p => p.PackageName)
                    .ToList();

                foreach (var pkg in chocoresult) Packages.Add(pkg);
            }
            catch (Exception e)
            {
                CTLog.AddError("ManualRefresh failed", e);
                MessageBox.Show("Refresh failed, see Log for more Information");
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}
