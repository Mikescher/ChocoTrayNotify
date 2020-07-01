using MSHC.WPF.MVVM;

namespace ChocoTrayNotify
{
    public class CTNSettings : ObservableObject
    {
        public int ChocoCommandTimeout { get { return _chocoCommandTimeout; } set { _chocoCommandTimeout = value; OnPropertyChanged(); } }
        private int _chocoCommandTimeout = 60 * 1000;

        public bool ShowPowershellWindow { get { return _showPowershellWindow; } set { _showPowershellWindow = value; OnPropertyChanged(); } }
        private bool _showPowershellWindow = false;

        public bool PowershellElevate { get { return _powershellElevate; } set { _powershellElevate = value; OnPropertyChanged(); } }
        private bool _powershellElevate = true;

        public string PowershellPath { get { return _powershellPath; } set { _powershellPath = value; OnPropertyChanged(); } }
        private string _powershellPath = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe";

        public string ChocoCommand { get { return _chocoCommand; } set { _chocoCommand = value; OnPropertyChanged(); } }
        private string _chocoCommand = @"choco";

        public bool SortUpdatesToTop { get { return _sortUpdatesToTop; } set { _sortUpdatesToTop = value; OnPropertyChanged(); } }
        private bool _sortUpdatesToTop = false;

        public bool StartMinimized { get { return _startMinimized; } set { _startMinimized = value; OnPropertyChanged(); } }
        private bool _startMinimized = false;

        public bool CheckForCTNUpdates { get { return _checkForCTNUpdates; } set { _checkForCTNUpdates = value; OnPropertyChanged(); } }
        private bool _checkForCTNUpdates = true;

        public int CheckForCTNUpdatesInterval { get { return _checkForCTNUpdatesInterval; } set { _checkForCTNUpdatesInterval = value; OnPropertyChanged(); } }
        private int _checkForCTNUpdatesInterval = 6 * 60 * 60 * 1000; // 6 hour

        public bool RefreshInBackground { get { return _refreshInBackground; } set { _refreshInBackground = value; OnPropertyChanged(); } }
        private bool _refreshInBackground = true;

        public int BackgroundRefreshInterval { get { return _backgroundRefreshInterval; } set { _backgroundRefreshInterval = value; OnPropertyChanged(); } }
        private int _backgroundRefreshInterval = 6 * 60 * 60 * 1000; // 6 hour

        public int InitialBackgroundRefreshDelay { get { return _initialBackgroundRefreshDelay; } set { _initialBackgroundRefreshDelay = value; OnPropertyChanged(); } }
        private int _initialBackgroundRefreshDelay = 15 * 60 * 1000; // 15 min

        public bool ShowBalloonOnChocoUpdatesFound { get { return _showBalloonOnChocoUpdatesFound; } set { _showBalloonOnChocoUpdatesFound = value; OnPropertyChanged(); } }
        private bool _showBalloonOnChocoUpdatesFound = true;

        public bool ShowBalloonOnCTNUpdatesFound { get { return _showBalloonOnCTNUpdatesFound; } set { _showBalloonOnCTNUpdatesFound = value; OnPropertyChanged(); } }
        private bool _showBalloonOnCTNUpdatesFound = true;

        public bool SimplifyPackageDisplayList { get { return _simplifyPackageDisplayList; } set { _simplifyPackageDisplayList = value; OnPropertyChanged(); } }
        private bool _simplifyPackageDisplayList = true;

        
    }
}
