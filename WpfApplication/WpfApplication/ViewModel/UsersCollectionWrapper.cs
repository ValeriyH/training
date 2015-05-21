using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication.Model;

namespace WpfApplication.ViewModel
{
    public class UsersCollectionWrapper : BaseViewModel, IEnumerable<IUserInfo>, INotifyCollectionChanged
    {
        public UsersCollectionWrapper()
        {
            App.Model.ModelChanged += () =>
            {
                OnPropertyChanged();
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            };
        }

        public IEnumerator<IUserInfo> GetEnumerator()
        {
            return App.Model.Users.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null) handler(this, e);
        }
    }
}
