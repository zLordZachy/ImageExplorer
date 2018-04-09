using System;
using System.Windows.Input;

namespace VektorovyEditor.helpers
{
    public class ZCommand : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<object> _execute;

        public ZCommand(Predicate<object> canExecute, Action<object> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }
        
        public event EventHandler CanExecuteChanged;
      
        public void ChangeCanExecute()
        {
            EventHandler canExecuteChanged = CanExecuteChanged;
            canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

     }
}