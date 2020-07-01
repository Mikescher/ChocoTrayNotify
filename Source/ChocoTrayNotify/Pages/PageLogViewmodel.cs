using ChocoTrayNotify.Log;
using MSHC.WPF.MVVM;
using System.Collections.ObjectModel;

namespace ChocoTrayNotify.Pages
{
    public class PageLogViewmodel: ObservableObject
    {
        public ObservableCollection<CTNLogEntry> Logs { get; } = GAS.Log.Entries;

        private CTNLogEntry _selectedLogItem = null;
        public CTNLogEntry SelectedLogItem
        {
            get { return _selectedLogItem; }
            set { _selectedLogItem = value; OnPropertyChanged(); }
        }
    }
}
