using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest
{
    public class AssemblyTest
    {
        private static Assembly assembly = Assembly.GetCallingAssembly();

        static public void Test()
        {
            ShowAssembly();
            Console.WriteLine("Init with " + assembly.GetName().Name);
        }

        static public void ShowAssembly()
        {
            //Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);
            //Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name);
            Console.WriteLine(Assembly.GetCallingAssembly().GetName().Name);
        }
    }
}
