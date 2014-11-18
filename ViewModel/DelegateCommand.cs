﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMApp
{
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;


        public DelegateCommand(Action<object> execute)
            : this(execute, null){
        }

        public DelegateCommand(Action<object> execute,
                       Predicate<object> canExecute){
            _execute = execute;
            _canExecute = canExecute;
        }



        public void RaiseCanExecuteChanged(){
           /* if (CanExecuteChanged != null){
                CanExecuteChanged(this, EventArgs.Empty);
            }*/
        }

        public bool CanExecute(object parameter){
            if (_canExecute == null){
                return true;
            }

            return _canExecute(parameter);
        }

        public void Execute(object parameter){
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

    }
}
