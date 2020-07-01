using ChocoTrayNotify.Choco;
using ChocoTrayNotify.Log;
using MSHC.WPF.MVVM;
using System.Collections.Generic;

namespace ChocoTrayNotify
{
    public static class GAS
    {
        public static GlobalAppState Inst     => GlobalAppState.Inst;
        public static CTNSettings    Settings => GlobalAppState.Inst.Settings;
        public static CTNLog         Log      => GlobalAppState.Inst.Log;
    }

    public class GlobalAppState: ObservableObject
    {
        public static GlobalAppState Inst => App.CurrentApp.State;

        public CTNSettings Settings { get; set; } = new CTNSettings();
        public CTNLog      Log      { get; set; } = new CTNLog();

        private AppStatus _appStatus = AppStatus.Okay;
        public AppStatus AppStatus
        { 
            get { return _appStatus; }
            set { _appStatus = value; OnPropertyChanged(); }
        }

        private List<ChocoPackage> _lastRefreshResult = new List<ChocoPackage>();
        public List<ChocoPackage> LastRefreshResult
        {
            get { return _lastRefreshResult; }
            set { _lastRefreshResult = value; OnPropertyChanged(); }
        }

        private List<ChocoPackage> _lastUpdateList = new List<ChocoPackage>();
        public List<ChocoPackage> LastUpdateList
        {
            get { return _lastUpdateList; }
            set { _lastUpdateList = value; OnPropertyChanged(); }
        }
    }
}
