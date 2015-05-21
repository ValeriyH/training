namespace WpfApplication.Model
{
    public interface IUserInfo
    {
        event UpdateEvent Updated;
        string User { get; set; }
        string Comment { get; set; }
    }

    public delegate void UpdateEvent();

    class UserInfo : IUserInfo
    {
        private string _user;
        private string _comment;

        public event UpdateEvent Updated;

        public UserInfo()
        {
        }

        public UserInfo(string user, string comment)
        {
            _user = user;
            _comment = comment;
        }

        private void FireUpdated()
        {
            UpdateEvent handler = Updated;
            if (handler != null) handler();
        }

        public string User
        {
            get { return _user; }
            set
            {
                _user = value;
                FireUpdated();
            }
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                FireUpdated();
            }

        }
    }
}
