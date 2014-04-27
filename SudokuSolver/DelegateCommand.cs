using System;
using System.Windows.Input;
namespace SudokuSolver
{
	public sealed class DelegateCommand
		: ICommand
	{
		public DelegateCommand(Action<object> execute)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");

			_execute = execute;
		}

		#region ICommand Members
		public void Execute(object parameter)
		{
			_execute(parameter);
		}
		public bool CanExecute(object parameter)
		{
			return true;
		}
		public event EventHandler CanExecuteChanged;
		#endregion

		private readonly Action<object> _execute;
	}
}