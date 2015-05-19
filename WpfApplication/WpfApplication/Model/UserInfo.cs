using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication.Model
{
    public interface IUserInfo
    {
        string User { get; set; }
        string Comment { get; set; }
    }

    class UserInfo : IUserInfo
    {
        public string User { get; set; }
        public string Comment { get; set; }
    }
}
