namespace ChocoTrayNotify.Pages
{
    public partial class PageLog
    {
        public PageLogViewmodel VM;

        public PageLog()
        {
            InitializeComponent();
            RootGrid.DataContext = VM = new PageLogViewmodel();
        }
    }
}
