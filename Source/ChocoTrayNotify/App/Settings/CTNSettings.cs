using ChocoTrayNotify.Serialization;
using MSHC.WPF.MVVM;
using System;
using System.IO;

namespace ChocoTrayNotify
{
    public class CTNSettings : ObservableObject, ICTNSerializable
    {
        [CTNXMLField]
        public int ChocoCommandTimeout { get { return _chocoCommandTimeout; } set { _chocoCommandTimeout = value; OnPropertyChanged(); } }
        private int _chocoCommandTimeout = 60 * 1000;

        [CTNXMLField]
        public bool ShowPowershellWindow { get { return _showPowershellWindow; } set { _showPowershellWindow = value; OnPropertyChanged(); } }
        private bool _showPowershellWindow = false;

        [CTNXMLField]
        public bool PowershellElevate { get { return _powershellElevate; } set { _powershellElevate = value; OnPropertyChanged(); } }
        private bool _powershellElevate = true;

        [CTNXMLField]
        public string PowershellPath { get { return _powershellPath; } set { _powershellPath = value; OnPropertyChanged(); } }
        private string _powershellPath = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe";

        [CTNXMLField]
        public string ChocoCommand { get { return _chocoCommand; } set { _chocoCommand = value; OnPropertyChanged(); } }
        private string _chocoCommand = @"choco";

        [CTNXMLField]
        public bool SortUpdatesToTop { get { return _sortUpdatesToTop; } set { _sortUpdatesToTop = value; OnPropertyChanged(); } }
        private bool _sortUpdatesToTop = false;

        [CTNXMLField]
        public bool StartMinimized { get { return _startMinimized; } set { _startMinimized = value; OnPropertyChanged(); } }
        private bool _startMinimized = true;

        [CTNXMLField]
        public bool CheckForCTNUpdates { get { return _checkForCTNUpdates; } set { _checkForCTNUpdates = value; OnPropertyChanged(); } }
        private bool _checkForCTNUpdates = true;

        [CTNXMLField]
        public int CheckForCTNUpdatesInterval { get { return _checkForCTNUpdatesInterval; } set { _checkForCTNUpdatesInterval = value; OnPropertyChanged(); } }
        private int _checkForCTNUpdatesInterval = 24 * 60; // [in minutes] // 1 day

        [CTNXMLField]
        public bool RefreshInBackground { get { return _refreshInBackground; } set { _refreshInBackground = value; OnPropertyChanged(); } }
        private bool _refreshInBackground = true;

        [CTNXMLField]
        public int BackgroundRefreshInterval { get { return _backgroundRefreshInterval; } set { _backgroundRefreshInterval = value; OnPropertyChanged(); } }
        private int _backgroundRefreshInterval = 6 * 60; // [in minutes] // 6 hour

        [CTNXMLField]
        public int InitialBackgroundRefreshDelay { get { return _initialBackgroundRefreshDelay; } set { _initialBackgroundRefreshDelay = value; OnPropertyChanged(); } }
        private int _initialBackgroundRefreshDelay = 15; // [in minutes] // 15 min

        [CTNXMLField]
        public bool ShowBalloonOnChocoUpdatesFound { get { return _showBalloonOnChocoUpdatesFound; } set { _showBalloonOnChocoUpdatesFound = value; OnPropertyChanged(); } }
        private bool _showBalloonOnChocoUpdatesFound = true;

        [CTNXMLField]
        public bool ShowBalloonOnCTNUpdatesFound { get { return _showBalloonOnCTNUpdatesFound; } set { _showBalloonOnCTNUpdatesFound = value; OnPropertyChanged(); } }
        private bool _showBalloonOnCTNUpdatesFound = true;

        [CTNXMLField]
        public bool SimplifyPackageDisplayList { get { return _simplifyPackageDisplayList; } set { _simplifyPackageDisplayList = value; OnPropertyChanged(); } }
        private bool _simplifyPackageDisplayList = true;

        [CTNXMLField]
        public bool SendAnonStatistics { get { return _sendAnonStatistics; } set { _sendAnonStatistics = value; OnPropertyChanged(); } }
        private bool _sendAnonStatistics = true;

        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private static readonly CTNXMLSerializer<CTNSettings> _serializer = new CTNXMLSerializer<CTNSettings>("configuration");

        private readonly string _path;

        public CTNSettings(string path)
        {
            _path = path;
        }

        public void Save()
        {
            File.WriteAllText(_path, Serialize());
        }

        public static CTNSettings Load(string path)
        {
            return Deserialize(File.ReadAllText(path), path);
        }

        public string Serialize()
        {
            return _serializer.Serialize(this, CTNXMLSerializer<CTNSettings>.DEFAULT_SERIALIZATION_SETTINGS);
        }

        public CTNSettings Clone()
        {
            var clone = new CTNSettings(_path);
            _serializer.Clone(this, clone);
            return clone;
        }

        internal void Apply(CTNSettings source)
        {
            _serializer.Clone(source, this);
        }

        public static CTNSettings Deserialize(string xml, string path)
        {
            var r = new CTNSettings(path);
            _serializer.Deserialize(r, xml, CTNXMLSerializer<CTNSettings>.DEFAULT_SERIALIZATION_SETTINGS);
            return r;
        }

        public void OnBeforeSerialize()
        {
            //
        }

        public void OnAfterDeserialize()
        {
            //
        }

    }
}
