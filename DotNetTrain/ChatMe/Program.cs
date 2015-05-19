using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ChatMe
{
    class Settings
    {
        static Settings()
        {
            host = "127.0.0.1";
            port = 55555;
            user = "Not set";
        }

        public static int port { get; set; }
        public static string host { get; set; }
        public static string user { get; set; }

        public static bool IsPortValid(int port, out string error)
        {
            if (port >= 49152 && port <= 65535)
            {
                //the ports 49152-65535 are reserved by IANA for private use
                error = "";
                return true;
            }
            error = "Port should be a number beetween 49152 and 65535";
            return false;
        }
        public static bool TrySetPort(string s, out string error)
        {
            int i;
            try
            {
                i = int.Parse(s);
            }
            catch
            {
                error = String.Format("Can't convert '{0}' to port. Port should be a number", s);
                return false;
            }
            if (Settings.IsPortValid(i, out error))
            {
                port = i;
                return true;
            }
            return false;
        }
    }

    class Connection : IDisposable
    {
        TcpClient client;

        public Connection()
        {
            client = new TcpClient(Settings.host, Settings.port);
        }

        public void SendMessage(string message)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream);
                writer.Write(message);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public string ReceiveMessage()
        {
            string message = "";
            try
            {
                NetworkStream stream = client.GetStream();
                System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);
                message = reader.ReadString();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            return message;

        }

        public bool IsReady()
        {
            return client.GetStream().DataAvailable;
        }

        public void Dispose()
        {
            client.GetStream().Close();
            client.Close();
        }
    }

    class Chat
    {
        public Chat()
        {
        }

        ConsoleColor GetColorByUser(string user)
        {
            if (user == "")
            {
                //System message
                return ConsoleColor.DarkYellow;
            }
            int i = user[0];
            i = i % 7 + 9;

            ConsoleColor ret = (ConsoleColor) i;
            return ret;
        }

        string GetMessageUser(string message)
        {
            string[] array = message.Split(':');

            if (array.Length > 1)
            {
                return array[0];
            }
            return "";
        }

        public void DoChat()
        {
            using (Connection connection = new Connection())
            {
                connection.SendMessage(Settings.user);
                do
                {
                    if (connection.IsReady())
                    {
                        string message = connection.ReceiveMessage();
                        if (message == "admin:exit")
                        {
                            Console.WriteLine("Exiting...");
                            break;
                        }
                        string user = GetMessageUser(message);
                        Console.ForegroundColor = GetColorByUser(user);
                        Console.WriteLine(message);
                    }

                    if (Console.KeyAvailable)
                    {
                        Console.ResetColor();
                        Console.Write("{0}:", Settings.user);
                        string message = Console.ReadLine();
                        message = String.Format("{0}:{1}", Settings.user, message);
                        connection.SendMessage(message);
                        if (message == String.Format("{0}:exit", Settings.user))
                        {
                            break;
                        }
                    }

                    System.Threading.Thread.Sleep(500);
                }
                while (true);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Connection con = new Connection();
            //con.SendMessage("Hello");
            //con.SendMessage("Hello Again");


            if (args.Length > 0)
            {
                Settings.host = args[0];
            }
            if (args.Length > 1)
            {
                string error;
                if (!Settings.TrySetPort(args[1], out error))
                {
                    Console.WriteLine("ERROR: {0}", error);
                    return;
                }
            }
            Console.Write("Please enter your name: ");
            Settings.user = Console.ReadLine();

            Console.WriteLine("Establishing connection to server {0}:{1}", Settings.host, Settings.port);

            Chat chat = new Chat();
            chat.DoChat();
        }
    }
}
