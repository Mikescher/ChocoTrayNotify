using System.Windows.Controls;

namespace ChocoTrayNotify.Pages
{
    public partial class PageLog
    {
        public PageLog()
        {
            InitializeComponent();
            RootGrid.DataContext = new PageLogViewmodel();
        }
    }
}
