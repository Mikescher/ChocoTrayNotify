using System.Windows.Controls;

namespace ChocoTrayNotify.Pages
{
    public partial class PageSettings
    {
        public PageSettings()
        {
            InitializeComponent();
            RootGrid.DataContext = new PageSettingsViewmodel();
        }
    }
}
