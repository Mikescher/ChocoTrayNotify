namespace ChocoTrayNotify.Pages
{
    public partial class PageStatus
    {
        public PageStatusViewmodel VM;

        public PageStatus()
        {
            InitializeComponent();
            RootGrid.DataContext = VM = new PageStatusViewmodel();
        }
    }
}
