using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApplication.ViewModel
{
    class AddCommand : ICommand
    {
        private readonly UserViewModel _model;

        public AddCommand(UserViewModel model)
        {
            _model = model;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string user = _model.User;
            string comment = _model.Comment;
            App.Model.Add(user, comment);
            if (_model.CloseAction != null)
            {
                _model.CloseAction();
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
