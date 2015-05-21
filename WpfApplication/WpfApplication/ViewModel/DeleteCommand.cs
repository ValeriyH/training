﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApplication.Model;

namespace WpfApplication.ViewModel
{
    class DeleteCommand : ICommand
    {
        private MainWindowViewModel _viewModel;
        public DeleteCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            //Only if some items selected
            return true;
        }

        public void Execute(object parameter)
        {
            IEnumerable collectioin = parameter as IEnumerable;
            if (collectioin != null)
            {
                App.Model.Remove(collectioin);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
