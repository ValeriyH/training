using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Input;
using WpfApplication.Model;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Refresh();
        }

        private void Refresh()
        {
            UserList.Items.Clear();
            foreach (var item in App.Model.Users)
            {
                UserList.Items.Add(item);
            }
            UserList.Items.Refresh();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            User sUser = new User {Owner = this, ResizeMode = ResizeMode.NoResize, Title = "Add User..."};
            if (sUser.ShowDialog() == true)
            {
                Refresh();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            IUserInfo data = UserList.SelectedItem as IUserInfo;
            User sUser = new User { Owner = this, ResizeMode = ResizeMode.NoResize, Title = "Edit User...", UserData = (IUserInfo)UserList.SelectedItem};
            if (sUser.ShowDialog() == true)
            {
                Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            App.Model.Remove(UserList.SelectedItems);
            Refresh();
        }

        private void UserList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Edit_Click(sender, e);
        }
    }
}
