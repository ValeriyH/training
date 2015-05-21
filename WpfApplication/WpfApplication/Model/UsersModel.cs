using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Script.Serialization;

namespace WpfApplication.Model
{
    public interface IUserModel  : IDisposable
    {
        IEnumerable<IUserInfo> Users { get; }
        event UpdateEvent ModelChanged;

        void Add(string user, string comment);
        void Remove(IUserInfo item);
        void Remove(IEnumerable items);
    }

    class UsersModel : IUserModel
    {
        private List<UserInfo> usersList;
        public event UpdateEvent ModelChanged;

        IEnumerable<IUserInfo> IUserModel.Users
        {
            get { return usersList; }
        }

        public UsersModel()
        {
            RestoreList();
        }

        public void Dispose()
        {
            StoreList();
        }

        public void Add(string user, string comment)
        {
            UserInfo userInfo = new UserInfo(user, comment);
            usersList.Add(userInfo);
            userInfo.Updated += FireModelChanged;
            FireModelChanged();
        }

         public void Remove(IUserInfo item)
         {
             usersList.Remove(item as UserInfo);
             FireModelChanged();
         }

         public void Remove(IEnumerable items)
         {
             List<IUserInfo> removing = new List<IUserInfo>();

             foreach (var item in items)
             {
                 removing.Add(item as IUserInfo);
             }

             foreach (var item in removing)
             {
                 Remove(item);
             }
         }

         private void FireModelChanged()
         {
             UpdateEvent handler = ModelChanged;
             if (handler != null) handler();
         }

         private void RestoreList()
        {
            try
            {
                var jsonSerialiser = new JavaScriptSerializer();
                string data = File.ReadAllText("data.txt");
                usersList = jsonSerialiser.Deserialize<List<UserInfo>>(data);
                foreach (var user in usersList)
                {
                    user.Updated += FireModelChanged;
                }
                FireModelChanged();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                usersList = new List<UserInfo>();
            }
        }

        private void StoreList()
        {
            try
            {
                var jsonSerialiser = new JavaScriptSerializer();
                var result = jsonSerialiser.Serialize(usersList);
                File.WriteAllText("data.txt", result);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
