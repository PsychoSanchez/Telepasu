using Proxy.ServerEntities;
using Proxy.ServerEntities.SQL;
using Proxy.ServerEntities.Users;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Proxy
{
    public class Server
    {
        public static readonly MailPost MailPost = new MailPost();
        private IPEndPoint _endpoint;
        TcpListener _listener;
        readonly ManualResetEvent _tcpClientConnected = new ManualResetEvent(false);
        bool _stopProxy = false;

        public void Start()
        {
            Init();
            while (!_stopProxy)
            {
                AcceptClient();
            }
        }
        public void Stop()
        {
            _stopProxy = true;
            _listener.Stop();
            DisconnectAll();
        }
        /// <summary>
        /// Инициализация подключения к сереверу
        /// </summary>
        public void Init()
        {
            //IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, Int32.Parse(tbPortNumber.Text));
            //endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            _endpoint = new IPEndPoint(IPAddress.Any, 5000);
            _listener = new TcpListener(_endpoint);
            _listener.Start();
            telepasu.log("#Server initialized...");
        }

        /// <summary>
        /// Функция подключения к астериску
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool ConnectAsterisk(string username, string password, string ip, int port)
        {
            Socket socket = GetSocket();
            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ip), Convert.ToInt32(port));
                socket.Connect(endpoint);
                AsteriskEntity asterisk = new AsteriskEntity(socket);
                if (asterisk.Login(username, password))
                {
                    telepasu.log("#Asterisk connected...");
                    MailPost.AddNativeModule("AsteriskServer1",asterisk);
                    ThreadPool.QueueUserWorkItem(AsteriskThread, MailPost.Asterisk);
                    return true;
                }

                telepasu.log("#Failed to connect asterisk...");
                return false;
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.TimedOut) return false;

                telepasu.log("Сервер недоступен");
                throw new Exception("Сервер недоступен");
            }
        }
        public void DisconnectAsterisk()
        {
            if (MailPost.Asterisk == null) return;

            MailPost.Asterisk.Logoff();
            MailPost.DeleteUser(MailPost.Asterisk);
            MailPost.Asterisk = null;
        }
        public List<string> ShowConnectedUsers()
        {
            List<string> list = new List<string>();
            var users = MailPost.GetUsers();
            foreach (var user in users)
            {
                list.Add(user.UserName);
            }
            return list;
        }
        void AsteriskThread(object obj)
        {
            UserManager asterisk = (UserManager)obj;
            asterisk.StartWork();
        }
        public bool ConnectDatabase(string username, string password, string ip, int port)
        {
            IDB db = new FakeDB();
            if (db.ConnectDB("123", "123", "123", "123"))
            {
                MailPost.AddDb(db);
                telepasu.log("#Database connected...");
                return true;
            }
            telepasu.log("#Failed to connect database...");
            return false;
        }
        private Socket GetSocket()
        {
            Socket newsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                ExclusiveAddressUse = true,
                SendTimeout = 30000,
                ReceiveTimeout = 70000,
                Ttl = 42
            };
            // Don't allow another socket to bind to this port.
            // Timeout 3 seconds
            // Set the Time To Live (TTL) to 42 router hops.
            return newsocket;
        }
        public void AcceptClient()
        {
            telepasu.log("#Waiting for client...");
            _tcpClientConnected.Reset();
            if (_stopProxy)
            {
                return;
            }
            _listener.BeginAcceptTcpClient(ProcessIncomingConnection, _listener);
            _tcpClientConnected.WaitOne();
        }
        void ProcessIncomingConnection(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            UserManager serverUser = new GuestEntity(listener.AcceptTcpClient(), listener.EndAcceptSocket(ar), 5000);
            //Проверка наличия айпи в белом листе
            //var oipi = ((IPEndPoint)temp.client.Client.RemoteEndPoint).Address.ToString();
            //if(ipTable.Compare(oipi)){
            ThreadPool.QueueUserWorkItem(ProcessIncomingData, serverUser);
            // TODO: Get threads count
            //ThreadPool.GetMaxThreads() ThreadPool.GetAvailableThreads()
            //telepasu.log();
            //}
            _tcpClientConnected.Set();
        }
        void ProcessIncomingData(object obj)
        {
            GuestEntity guest = (GuestEntity)obj;
            guest.AuthorizationOver += Guest_AuthorizationOver;
            guest.BeginAutorization();
            telepasu.log("Client thread stoped...");
        }

        private void Guest_AuthorizationOver(object sender, AuthEventArgs e)
        {
            Console.WriteLine(sender as GuestEntity);
            if (!e.Authentificated)
            {
                telepasu.log("Authentification failed...\r\nClient kicked.");
                return;
            }

            telepasu.log("Client accepted...");
            MailPost.Add(e.Client);
            e.Client.StartWork();
        }

        public void DisconnectAll()
        {
            foreach (var user in MailPost.GetUsers())
            {
                user.Shutdown();
            }
            MailPost.Clear();
        }
    }
}
