using ChocoTrayNotify.Choco;
using ChocoTrayNotify.Log;
using MSHC.WPF.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ChocoTrayNotify.Pages
{
    public class PageStatusViewmodel: ObservableObject
    {
        public ICommand RefreshCommand => new RelayCommand(ManualRefresh);

        public ObservableCollection<ChocoPackage> Packages { get; set; } = new ObservableCollection<ChocoPackage>();

        private bool _isRefreshing = false;
        public bool IsRefreshing
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

                if (CTNSettings.Inst.SortUpdatesToTop)
                {
                    chocoresult = chocoresult
                        .OrderByDescending(p => p.UpdateAvailable && p.Pinned != true)
                        .ThenBy(p => p.Pinned)
                        .ThenBy(p => p.PackageName)
                        .ToList();
                }
                else
                {
                    chocoresult = chocoresult
                        .OrderBy(p => p.PackageName)
                        .ToList();
                }


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
