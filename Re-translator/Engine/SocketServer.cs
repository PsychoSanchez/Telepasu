using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Proxy.ServerEntities;
using Proxy.ServerEntities.Application;

namespace Proxy.Engine
{
    public class SocketServer
    {
        private IPEndPoint _endpoint;
        private TcpListener _listener;
        private readonly ManualResetEvent _tcpClientConnected = new ManualResetEvent(true);
        private bool _stopProxy = false;
        private const string ModuleName = "#(Socket Server) ";

        //public async void Start()
        //{
        //    Init();
        //    while (!_stopProxy)
        //    {
        //       //await AcceptClient();
        //    }
        //}
        public void Stop()
        {
            _tcpClientConnected.Set();
            _stopProxy = true;
            _listener.Stop();
            //DisconnectAll();
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
            telepasu.log(ModuleName + "#SocketServer initialized...");
        }
         public Socket GetSocket()
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
        /// <summary>
        /// Функция подключения к астериску
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        //public bool ConnectAsterisk(string username, string password, string ip, int port)
        //{

        //}
        public void DisconnectAsterisk()
        {
            //if (MailPost.Asterisk == null) return;

            //MailPost.Asterisk.Logoff();
            //MailPost.DeleteUser(MailPost.Asterisk);
            //MailPost.Asterisk = null;
        }
        //public List<string> ShowConnectedUsers()
        //{
        //    //List<string> list = new List<string>();
        //    //var users = MailPost.GetUsers();
        //    //foreach (var user in users)
        //    //{
        //    //    list.Add(user.UserName);
        //    //}
        //    //return list;
        //}
        void AsteriskThread(object obj)
        {
            EntityManager asterisk = (EntityManager)obj;
            asterisk.StartWork();
        }
        //public bool ConnectDatabase(string username, string password, string ip, int port)
        //{
        //    IDB db = new FakeDB();
        //    if (db.ConnectDB("123", "123", "123", "123"))
        //    {
        //        MailPost.AddDb(db);
        //        telepasu.log("#Database connected...");
        //        return true;
        //    }
        //    telepasu.log("#Failed to connect database...");
        //    return false;
        //}

        public void AcceptClient()
        {
            if (_stopProxy)
            {
                return;
            }
            if (_listener.Pending())
            {
                telepasu.log(ModuleName + "Accepting client...");
                _listener.BeginAcceptTcpClient(ProcessIncomingConnection, _listener);
            }
            _tcpClientConnected.WaitOne(500);
        }
        void ProcessIncomingConnection(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            try
            {
                EntityManager serverEntity = new GuestEntity(listener.EndAcceptTcpClient(ar), 5000);
                ThreadPool.QueueUserWorkItem(ProcessIncomingData, serverEntity);
            }
            catch (Exception e)
            {
                telepasu.exc(e);
            }
            //Проверка наличия айпи в белом листе
            //var oipi = ((IPEndPoint)temp.client.Client.RemoteEndPoint).Address.ToString();
            //if(ipTable.Compare(oipi)){
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
            telepasu.log(ModuleName + "Authorization started...");
        }

        private void Guest_AuthorizationOver(object sender, AuthEventArgs e)
        {
            if (!e.Authentificated)
            {
                telepasu.log(ModuleName + "Authentification failed...\r\nClient kicked.");
                return;
            }

            telepasu.log(ModuleName + "Client accepted...");
            ProxyEngine.MailPost.AddApplication(e.Client.UserName, e.Client);
            e.Client.StartWork();
        }

        //public void DisconnectAll()
        //{
        //    foreach (var user in MailPost.GetUsers())
        //    {
        //        user.Shutdown();
        //    }
        //    MailPost.Clear();
        //}
    }
}
