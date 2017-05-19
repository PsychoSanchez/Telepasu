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

        public void DisconnectModule()
        {
        }
        //public List<string> ShowConnectedUsers()
        //{
        //}

        public void DisconnectApplication()
        {
            
        }

        void AsteriskThread(object obj)
        {
            EntityManager asterisk = (EntityManager)obj;
            asterisk.StartWork();
        }

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
    }
}
