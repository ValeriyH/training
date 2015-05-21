using System.Windows.Input;

namespace WpfApplication.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private AddPopupCommand _addCommand;
        private EditPopupCommand _editCommand;
        private DeleteCommand _deleteCommand;
        private UsersCollectionWrapper users;

        public MainWindowViewModel()
        {
            _addCommand = new AddPopupCommand(this);
            _editCommand = new EditPopupCommand(this);
            _deleteCommand = new DeleteCommand(this);
            users = new UsersCollectionWrapper();
        }

        public ICommand AddButton
        {
            get { return _addCommand;  }
        }

        public ICommand EditButton
        {
            get { return _editCommand; }
        }

        public ICommand DeleteButton
        {
            get { return _deleteCommand; }
        }

        public UsersCollectionWrapper Users
        {
            get
            {
                return users;

            }
        } 
    }
}
