using System;
using System.Windows.Input;

namespace Clever.ViewModel.BaseVM
{
    public class RelayCommand : ICommand
    {
        private Action<Object> _action;
        private bool _canExecute;
        public RelayCommand(Action<Object> action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action(parameter);
        }

    }
}
