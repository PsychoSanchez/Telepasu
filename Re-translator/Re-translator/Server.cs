
using Proxy.ServerEntities;
using Proxy.ServerEntities.UserEntities;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Proxy
{
    public class Server
    {
        public static DeMail Mail = new DeMail();
        IPEndPoint endpoint;
        TcpListener listener;
        int ThreadCount = 0;
        ManualResetEvent tcpClientConnected = new ManualResetEvent(false);
        bool StopProxy = false;

        public void initInnerThreads()
        {
        }
        public void Start()
        {
            init();
            while (!StopProxy)
            {
                AcceptClient();
            }
        }
        public void Stop()
        {
            StopProxy = true;
            listener.Stop();
            foreach (var User in Mail.GetUsers())
            {
                User.Stop();
            }
        }
        /// <summary>
        /// Инициализация подключения к сереверу
        /// </summary>
        public void init()
        {
            //IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, Int32.Parse(tbPortNumber.Text));
            endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            listener = new TcpListener(endpoint);
            listener.Start();
            Console.WriteLine("#Server initialized...");
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
            return true;
        }
        public bool ConnectAsterisk(string username, string password, string domain)
        {
            return true;
        }
        public void AcceptClient()
        {
            Console.WriteLine("#Waiting for client...");
            tcpClientConnected.Reset();
            if (StopProxy)
            {
                return;
            }
            listener.BeginAcceptTcpClient(new AsyncCallback(ProcessIncomingConnection), listener);
            tcpClientConnected.WaitOne();
        }
        void ProcessIncomingConnection(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            GuestEntity ServerUser = new GuestEntity(listener.EndAcceptSocket(ar));
            //Проверка наличия айпи в белом листе
            //var oipi = ((IPEndPoint)temp.client.Client.RemoteEndPoint).Address.ToString();
            //if(ipTable.Compare(oipi)){
            ThreadPool.QueueUserWorkItem(ProcessIncomingData, ServerUser);
            //}

            tcpClientConnected.Set();
        }
        void ProcessIncomingData(object obj)
        {
            GuestEntity temp = (GuestEntity)obj;
            //try
            //{
            ServerEntity newEntity = temp.StartAutorization();
            Mail.AddUser(newEntity);
            newEntity.StartWork();
            //}
            //catch
            //{
            ///Exception
            //}
            //finally
            //{
            ///Restart Thread
            //}
        }
        public void DisconnectAll()
        {
            foreach (var user in Mail.GetUsers())
            {
                user.Stop();
            }
            Mail.Clear();
        }
    }
}
