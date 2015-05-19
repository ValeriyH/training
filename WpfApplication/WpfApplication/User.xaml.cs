using System;
using System.Windows;
using WpfApplication.Model;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Window
    {
        public User()
        {
            InitializeComponent();
        }

        public IUserInfo UserData { get; set; }

        protected override void OnActivated(EventArgs e)
        {
            if (UserData != null)
            {
                UserBox.Text = UserData.User;
                CommentBox.Text = UserData.Comment;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (UserData == null)
            {
                //Add dialog
                App.Model.CreateUserInfo(UserBox.Text, CommentBox.Text);
            }
            else
            {
                //Edit dialog
                UserData.User = UserBox.Text;
                UserData.Comment = CommentBox.Text;
            }
            DialogResult = true;
            Close();
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
