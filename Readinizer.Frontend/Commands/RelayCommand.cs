using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Readinizer.Frontend.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            if (execute == null) throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;
        public void Execute(object parameter) => _execute();
        // event an CommandManager delegieren (Benachrichtigung
        // erfolgt so immer dann, wenn WPF denkt, dass sich etwas
        // am Ausführungsstatus geändert hat, z.B. bei Key- oder
        // Mouse Button-Klick)
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
