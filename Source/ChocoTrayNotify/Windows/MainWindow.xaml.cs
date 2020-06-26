using System.Windows;

namespace ChocoTrayNotify.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewmodel();
        }
    }
}
