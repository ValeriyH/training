using System;
using System.Windows;
using System.Windows.Controls;
using WpfApplication.Model;
using WpfApplication.ViewModel;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Window
    {
        public User(UserViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}
