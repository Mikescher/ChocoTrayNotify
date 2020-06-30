using ChocoTrayNotify.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;

namespace ChocoTrayNotify
{
    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            (Application.Current.MainWindow = new MainWindow()).Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose();
            base.OnExit(e);
        }
    }
}
