using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApplication.Model;

namespace WpfApplication.ViewModel
{
    public abstract class UserViewModel : BaseViewModel
    {
        public string User { get; set; }
        public string Comment { get; set; }

        public Action CloseAction { get; set; }
        public abstract ICommand OkButton { get; }
        public ICommand CancelButton { get; set; }


        public UserViewModel()
        {
            CancelButton = new CloseCommand(this);
        }
    }

    class UserAddViewModel : UserViewModel
    {
        private readonly AddCommand _add;

        public UserAddViewModel()
        {
            _add = new AddCommand(this);
        }

        public override ICommand OkButton { get { return _add; } }
    }

    class UserEditViewModel : UserViewModel
    {
        private readonly EditCommand _edit;

        public UserEditViewModel(IUserInfo user)
        {
            User = user.User;
            Comment = user.Comment;
            _edit = new EditCommand(this, user);
        }

        public override ICommand OkButton { get { return _edit; } }
    }

}
