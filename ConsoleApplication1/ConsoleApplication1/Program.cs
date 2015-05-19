using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        // (x ^ y) % d
        //This function is useful for cryptography. mod by exponense is used with public/private keys https://www.youtube.com/watch?annotation_id=annotation_622705&feature=iv&src_vid=3QnD2c4Xovk&v=YEBfamv-_do
        static int TakeMod(int x, int y, int d)
        {
            if (x >= d)
            {
                throw  new Exception("x must be < d");
            }

            //performance can be improved, by reducing loop number
            //e.g 3^10 % 7 = (3^5 * 3^5) % 7 = ((3^5  % 7) * (3^5  % 7)) % 7 = (5 * 5) % 7 = 4
            //or 3^10 % 7 = (3^3 % 7 * 3^3 % 7 * 3^3 % 7 * 3 % 7) % 7 = (6 * 6 * 6 * 3) %7 = 4
            int res = x;
            for (int i = 1; i < y; i++)
            {
                res = res % d;
                res = res * x;
            }
            return res % d;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Take mod from full result");
            for (int i = 1; i < 15; i++)
            {
                int result = (int)Math.Pow(3, i);
                result = result % 7;
                Console.WriteLine("(3 ^ {0}) % 7 = {1}", i, result);
                Console.WriteLine("Taking mod: (3 ^ {0}) % 7 = {1}", i, TakeMod(3, i, 7));
            }

            //Console.WriteLine("Take mod from mod");
            //int res = 3;
            //for (int i = 1; i < 15; i++)
            //{
            //    res = res % 7;
            //    Console.WriteLine("(3 ^ {0}) % 7 = {1}", i, res);
            //    res = res*3;
            //}

            //Console.WriteLine("Taking mod");
            //Console.WriteLine("Result {0}", TakeMod(354864613, Int32.MaxValue, 1354864613));
            
        }
    }
}
