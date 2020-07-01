using ChocoTrayNotify.Choco;
using ChocoTrayNotify.Log;
using MSHC.WPF.MVVM;
using System;
using System.Collections.Generic;
using System.IO;

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
        public static readonly string PATH_SETTINGS    = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ctn.config");
        public static readonly string PATH_CHOCO_CACHE = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ctn.choco.cache");

        public static GlobalAppState Inst => App.CurrentApp.State;

        public CTNSettings Settings { get; set; }
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

        public readonly bool IsFirstStart;

        public GlobalAppState()
        {
            CTNSettings settings;
            try
            {
                if (File.Exists(PATH_SETTINGS))
                {
                    Log.AddDebug("Loading settings from file", PATH_SETTINGS);

                    settings = CTNSettings.Load(PATH_SETTINGS);
                    IsFirstStart = false;

                    Log.AddDebug("Settings loaded", settings.Serialize());
                }
                else
                {
                    Log.AddInfo("No settings file found - creating new one");

                    settings = new CTNSettings(PATH_SETTINGS);
                    settings.Save();
                    IsFirstStart = true;
                }

                Settings = settings;
            }
            catch (Exception e)
            {
                Log.AddError("Could not load settings", e);
                Settings = new CTNSettings(PATH_SETTINGS);
            }
        }
    }
}
