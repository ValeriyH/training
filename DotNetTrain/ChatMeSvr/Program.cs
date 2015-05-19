using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ChatMeSvr
{
    class Settings
    {
        static Settings()
        {
            host = "127.0.0.1";
            port = 55555;
        }

        public static int port { get; set; }
        public static string host { get; set; }

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

    class Server
    {
        delegate void ClientSendMessageDelegate(Client sender, string message);
        event ClientSendMessageDelegate NewMessageEvent;

        class Client: IDisposable
        {
            TcpClient connection;
            NetworkStream stream;

            public Client(TcpClient connection)
            {
                this.connection = connection;
                this.stream = connection.GetStream();
                User = ReadMessage();
            }

            public string User {private set; get;}
            public bool hasMessage
            {
                get
                {
                    try
                    {
                        return stream.DataAvailable;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    return false;
                }
            }

            public void OnNewMessage(Client sender, string message)
            {
                if (sender != this)
                {
                    SendMessage(message);
                }
            }

            public void SendMessage(string message)
            {
                try
                {
                    System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream);
                    writer.Write(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            public string ReadMessage()
            {
                string message = "";
                try
                {
                    if (hasMessage)
                    {
                        System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);
                        message = reader.ReadString();
                    }
                    else
                    {
                        Console.WriteLine("InternalError: Trying to read message from stream without data");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return message;
            }

            public void Dispose()
            {
                stream.Close();
                connection.Close();
                stream = null;
                connection = null;
            }
        }

        List<string> logoutUsers;
        Dictionary<string, Client> clients;
        TcpListener server = null;
        Task<TcpClient> accepTask;
        bool IsShutdownInitiated = false;


        public Server()
        {
            accepTask = null;
            logoutUsers = new List<string>();
        }

        public void Start()
        {
            try
            {
                IPAddress addr = IPAddress.Parse(Settings.host);
                server = new TcpListener(addr, Settings.port);

                // Start listening for client requests.
                server.Start();
                Console.WriteLine("ChatMe server started... ");

                accepTask = server.AcceptTcpClientAsync();
                clients = new Dictionary<string, Client>();

                // Enter the listening loop. 
                while (true)
                {
                    try
                    {
                        if (accepTask.IsCompleted)
                        {
                            Client client = new Client(accepTask.Result);
                            NewMessageEvent += client.OnNewMessage;
                            clients.Add(client.User, client);
                            Console.WriteLine("{0} connected...", client.User);

                            accepTask.Dispose();
                            accepTask = server.AcceptTcpClientAsync();
                        }

                        foreach (Client client in clients.Values)
                        {
                            if (client.hasMessage)
                            {
                                string data = client.ReadMessage();
                                data = ProcessCommand(data);
                                NewMessageEvent(client, data);
                                Console.WriteLine(data);
                            }
                        }
                        RemoveLogoutUsers();
                        if (IsShutdownInitiated)
                        {
                            Stop();
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO: remove problematic user from clients list
                        Console.WriteLine("Exeception: {0}", ex);
                    }
                    System.Threading.Thread.Sleep(500);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Exception");
                throw;
            }
            finally
            {
                if (server != null)
                {
                    server.Stop();
                }
                Console.WriteLine("Hit enter to continue...");
                Console.Read();
            }
        }

        public void Stop()
        {
            //if (accepTask != null)
            //{
            //    accepTask.Dispose();
            //    accepTask = null;
            //}
            NewMessageEvent(null, "admin:exit");
            if (clients != null)
            {
                foreach(Client client in clients.Values)
                {
                    NewMessageEvent -= client.OnNewMessage;
                    client.Dispose();
                }
                clients.Clear();
            }
            server.Stop();
        }

        private void RemoveLogoutUsers()
        {
            foreach(string user in logoutUsers)
            {
                NewMessageEvent -= clients[user].OnNewMessage;
                clients[user].Dispose();
                clients.Remove(user);
            }
            logoutUsers.Clear();
        }
        //if data is not command key - return data
        //instead return result of processed command
        string ProcessCommand(string data)
        {
            if (data == "admin:shutdown")
            {
                data = "ChatMe shutdown initiated...";
                IsShutdownInitiated = true;
            }
            else if (data.EndsWith(":exit"))
            {
                string user = data.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0];

                data = String.Format("{0} leave chat", user);
                logoutUsers.Add(user);
            }
            return data;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
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

            Console.WriteLine("Establishing server on {0}:{1}", Settings.host, Settings.port);

            Server server = new Server();
            server.Start();
        }
    }
}