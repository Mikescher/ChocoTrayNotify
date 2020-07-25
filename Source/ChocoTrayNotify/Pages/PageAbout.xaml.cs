namespace ChocoTrayNotify.Pages
{
    public partial class PageAbout
    {
        public PageAboutViewmodel VM;

        public PageAbout()
        {
            InitializeComponent();
            RootGrid.DataContext = VM = new PageAboutViewmodel();
        }
    }
}
