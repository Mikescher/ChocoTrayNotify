using System;
using System.Windows.Input;

namespace MSHC.WPF.MVVM
{
	/// <summary>
	/// http://stackoverflow.com/a/22286816/1761622
	/// </summary>
	public class RelayCommand : ICommand
	{
		readonly Action<object> _execute;
		readonly Predicate<object> _canExecute;

		public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");

			_execute = execute;
			_canExecute = canExecute;
		}

		public RelayCommand(Action execute, Predicate<object> canExecute = null)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");

			_execute = x => execute();
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			if (_canExecute == null) return true;
			return _canExecute.Invoke(parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
		}
	}
}
