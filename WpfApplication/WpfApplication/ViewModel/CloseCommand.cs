using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApplication.ViewModel
{
    class CloseCommand : ICommand
    {
        private UserViewModel _model;

        public CloseCommand(UserViewModel model)
        {
            _model = model;
        }

        public bool CanExecute(object parameter)
        {
            return _model.CloseAction != null;
        }

        public void Execute(object parameter)
        {
            if (_model.CloseAction != null)
            {
                _model.CloseAction();
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
