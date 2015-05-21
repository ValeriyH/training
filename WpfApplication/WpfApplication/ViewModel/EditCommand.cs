using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApplication.Model;

namespace WpfApplication.ViewModel
{
    class EditCommand : ICommand
    {
        private readonly UserViewModel _model;
        private readonly IUserInfo _user;

        public EditCommand(UserViewModel model, IUserInfo user)
        {
            _model = model;
            _user = user;
        }

        public bool CanExecute(object parameter)
        {
            //Only if one item selected
            return true;
        }

        public void Execute(object parameter)
        {
            string user = _model.User;
            string comment = _model.Comment;
            _user.User = user;
            _user.Comment = comment;
            if (_model.CloseAction != null)
            {
                _model.CloseAction();
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
