using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ChocoTrayNotify.Controls
{
    public partial class ProgressIndicatorControl : UserControl
    {
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(
            "Progress",
            typeof(int),
            typeof(ProgressIndicatorControl),
            new FrameworkPropertyMetadata(0));

        public int Progress
        {
            get { return (int)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        public ProgressIndicatorControl()
        {
            InitializeComponent();
            RootGrid.DataContext = this;

            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            timer.Tick += (e,a) => { Progress = (Progress + 1) % 4; };
            timer.Start();
        }
    }
}
