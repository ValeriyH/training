using System;
using System.Windows;
using System.Windows.Input;

namespace WpfApplication.ViewModel
{
    class AddPopupCommand : ICommand
    {
        private MainWindowViewModel _model;

        public AddPopupCommand(MainWindowViewModel mainWindowViewModel)
        {
            _model = mainWindowViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Window wnd = parameter as MainWindow;
            if (wnd != null)
            {
                UserViewModel model = new UserAddViewModel();
                User sUser = new User(model)
                {Owner = wnd, ResizeMode = ResizeMode.NoResize, Title = "Add User..."};
                model.CloseAction = new Action(sUser.Close);
                sUser.Show();
            }
            else
            {
                MessageBox.Show("Error: Wrong param!");
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
