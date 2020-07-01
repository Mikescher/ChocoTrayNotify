namespace ChocoTrayNotify.Pages
{
    public partial class PageSettings
    {
        public PageSettingsViewmodel VM;

        public PageSettings()
        {
            InitializeComponent();
            RootGrid.DataContext = VM = new PageSettingsViewmodel();
        }
    }
}
