using MSHC.WPF.MVVM;
using System.Windows;
using System.Windows.Input;

namespace ChocoTrayNotify
{
	public class NotifyIconViewModel
	{
		public ICommand ShowWindowCommand { get; } = new RelayCommand((_) => (Application.Current.MainWindow = new MainWindow()).Show() /*, (_) => Application.Current.MainWindow == null*/ );

		public ICommand HideWindowCommand { get; } = new RelayCommand((_) => Application.Current.MainWindow.Close() /*, (_) => Application.Current.MainWindow != null*/ );

		public ICommand ExitApplicationCommand { get; } = new RelayCommand(() => Application.Current.Shutdown());
	}
}
