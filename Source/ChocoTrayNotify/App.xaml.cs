using ChocoTrayNotify.Choco;
using ChocoTrayNotify.Log;
using ChocoTrayNotify.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace ChocoTrayNotify
{
    public partial class App : Application
    {
        public static App CurrentApp => (App)Current;

        public TaskbarIcon NotifyIcon = null;
        public NotifyIconViewmodel NotifyIconVM = null;

        public readonly GlobalAppState State;

        public App()
        {
            State = new GlobalAppState();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            NotifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            NotifyIconVM = (NotifyIconViewmodel)NotifyIcon.DataContext;

            if (!GAS.Settings.StartMinimized) (Application.Current.MainWindow = new MainWindow()).Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            NotifyIcon.Dispose();
            base.OnExit(e);
        }

        public async void ChocoRefresh(bool manual)
        {
            var win = Application.Current.MainWindow as MainWindow;
            var vm  = win?.StatusVM;

            try
            {
                State.AppStatus = AppStatus.Refreshing;

                if (manual && vm != null) vm.Packages.Clear();
                var chocoresult = await ChocoQueryExecutor.QueryFull();

                if (GAS.Settings.SortUpdatesToTop)
                {
                    chocoresult = chocoresult
                        .OrderByDescending(p => p.IsOutdatedAndUpdateable)
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

                if (vm != null) 
                {
                    vm.Packages.Clear();

                    if (GAS.Settings.SimplifyPackageDisplayList)
                        foreach (var pkg in ChocoUtil.CleanPackageList(chocoresult)) vm.Packages.Add(pkg);
                    else
                        foreach (var pkg in chocoresult) vm.Packages.Add(pkg);
                }

                var updates = chocoresult.Where(p => p.IsOutdatedAndUpdateable).ToList();
                if (updates.Any())
                {
                    if (!manual && GAS.Settings.ShowBalloonOnChocoUpdatesFound && updates.Any(u => State.LastUpdateList.All(p => p.PackageName != u.PackageName)))
                    {
                        ShowUpdateBalloon(chocoresult);
                    }

                    State.AppStatus = AppStatus.UpdatesAvailable;
                }
                else
                {
                    State.AppStatus = AppStatus.Okay;
                }

                GAS.Inst.LastUpdateList = updates;

                GAS.Inst.LastRefreshResult = chocoresult;
            }
            catch (Exception e)
            {
                GAS.Log.AddError("ManualRefresh failed", e);

                if (manual)
                    MessageBox.Show("Refresh failed, see Log for more Information");
                else
                    NotifyIcon.ShowBalloonTip("Error", "Refresh failed, see Log for more Information", BalloonIcon.Error);

                State.AppStatus = AppStatus.Error;
            }
        }

        private void ShowUpdateBalloon(List<ChocoPackage> packages)
        {
            var updates = ChocoUtil.CleanPackageList(packages.Where(p => p.IsOutdatedAndUpdateable).ToList());

            if (updates.Count <= 5)
            {
                var msg = string.Join("\n", updates.Select(p => $"{p.PackageName} ({p.CurrentVersion} -> {p.AvailableVersion})"));
                NotifyIcon.ShowBalloonTip("The following packages have new versions available", msg, BalloonIcon.Info);
            }
            else
            {
                NotifyIcon.ShowBalloonTip("New Updates", $"There are {updates.Count(p => p.UpdateAvailable && p.Pinned != true)} new updates available", BalloonIcon.Info);
            }
        }
    }
}
