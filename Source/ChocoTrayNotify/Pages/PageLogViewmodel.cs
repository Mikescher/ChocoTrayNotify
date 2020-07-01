using ChocoTrayNotify.Log;
using MSHC.WPF.MVVM;
using System.Collections.ObjectModel;

namespace ChocoTrayNotify.Pages
{
    public class PageLogViewmodel: ObservableObject
    {
        public ObservableCollection<CTLogEntry> Logs { get; } = CTLog.Inst.Entries;

        private CTLogEntry _selectedLogItem = null;
        public CTLogEntry SelectedLogItem
        {
            get { return _selectedLogItem; }
            set { _selectedLogItem = value; OnPropertyChanged(); }
        }
    }
}
