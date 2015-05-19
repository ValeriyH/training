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

        IUserInfo CreateUserInfo(string user, string comment);
        void Remove(IEnumerable items);
    }

     class UsersModel : IUserModel
    {
        private List<UserInfo> usersList;

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

        public IUserInfo CreateUserInfo(string user, string comment)
        {
            UserInfo userInfo = new UserInfo() {User = user, Comment = comment};
            usersList.Add(userInfo);
            return userInfo;
        }

         public void Remove(IEnumerable items)
         {
             foreach (var item in items)
             {
                 usersList.Remove(item as UserInfo);
             }
         }

         private void RestoreList()
        {
            try
            {
                var jsonSerialiser = new JavaScriptSerializer();
                string data = File.ReadAllText("data.txt");
                usersList = jsonSerialiser.Deserialize<List<UserInfo>>(data);
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
