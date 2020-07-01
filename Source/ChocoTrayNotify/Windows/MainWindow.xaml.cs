using ChocoTrayNotify.Pages;
using System.Windows;

namespace ChocoTrayNotify.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindowViewmodel VM;

        public PageStatusViewmodel StatusVM     => PageStatus.VM;
        public PageSettingsViewmodel SettingsVM => PageSettings.VM;
        public PageLogViewmodel LogVM           => PageLog.VM;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = VM = new MainWindowViewmodel();

            GAS.Log.AddDebug("Show Mainwindow");
        }

    }
}
