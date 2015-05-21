using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApplication.Model;

namespace WpfApplication.ViewModel
{
    class EditPopupCommand : ICommand
    {
        private MainWindowViewModel _model;

        public EditPopupCommand(MainWindowViewModel model)
        {
            _model = model;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MainWindow wnd = parameter as MainWindow;
            if (wnd != null)
            {
                IUserInfo selected = wnd.UserList.SelectedItem as IUserInfo;
                UserViewModel model = new UserEditViewModel(selected);
                User sUser = new User (model)
                {
                    Owner = wnd,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Edit User...",
                };
                model.CloseAction = new Action(sUser.Close);
                sUser.Show();
            }
            else
            {
                MessageBox.Show("Wrong parameter");
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
