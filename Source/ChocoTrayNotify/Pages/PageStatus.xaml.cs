using System.Windows.Controls;

namespace ChocoTrayNotify.Pages
{
    public partial class PageStatus
    {
        public PageStatus()
        {
            InitializeComponent();
            RootGrid.DataContext = new PageStatusViewmodel();
        }
    }
}
