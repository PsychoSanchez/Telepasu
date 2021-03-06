﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Event;
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
        public static int ConnectedUsers = 0;

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
            try
            {
                _listener.Start();
            }
            catch (SocketException e)
            {
                telepasu.log(ModuleName + "#Failed to initialize. Reason: " + e.Message);
                return;
            }
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
            _tcpClientConnected.Reset();
            if (_stopProxy)
            {
                return;
            }
            telepasu.log(ModuleName + "Accepting client...");
            _listener.BeginAcceptTcpClient(ProcessIncomingConnection, _listener);
            _tcpClientConnected.WaitOne();
        }
        void ProcessIncomingConnection(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            var tcp = listener.EndAcceptTcpClient(ar);

            //Проверка наличия айпи в белом листе
            //var oipi = ((IPEndPoint)tcp.Client.RemoteEndPoint).Address.ToString();
            //if (!ProxyEngine.LocalDb.CheckWhiteList(oipi))
            //{

            //    telepasu.log(ModuleName + "User kicked: access denied");
            //    _tcpClientConnected.Set();
            //    return;
            //}
            try
            {
                EntityManager serverEntity = new GuestEntity(tcp, 5000);
                ThreadPool.QueueUserWorkItem(ProcessIncomingData, serverEntity);
            }
            catch (Exception e)
            {
                telepasu.exc(e);
            }
            //if(ipTable.Compare(oipi)){
            // TODO: Get threads count
            //ThreadPool.GetMaxThreads() ThreadPool.GetAvailableThreads()
            //telepasu.log();
            //}
            _tcpClientConnected.Set();
        }
        void ProcessIncomingData(object obj)
        {
            ConnectedUsers++;
            telepasu.log(ModuleName + "Users connected: " + ConnectedUsers);
            GuestEntity guest = (GuestEntity)obj;
            guest.AuthorizationOver += Guest_AuthorizationOver;
            guest.BeginAutorization();
            telepasu.log(ModuleName + "Authorization started...");
        }

        private void Guest_AuthorizationOver(object sender, AuthEventArgs e)
        {
            GuestEntity guest = sender as GuestEntity;
            if (guest != null)
            {
                guest.AuthorizationOver -= Guest_AuthorizationOver;
            }
            if (!e.Authentificated)
            {
                telepasu.log(ModuleName + "Authentification failed... Client kicked.");
                ConnectedUsers--;
                if (ConnectedUsers < 1)
                {
                    ConnectedUsers = 1;
                }
                return;
            }
            ThreadPool.QueueUserWorkItem(ProcessUserData, e.Client);
        }

        private void ProcessUserData(object obj)
        {
            EntityManager client = (EntityManager)obj;
            telepasu.log(ModuleName + "Client accepted...");
            ProxyEngine.MailPost.AddApplication(client.UserName, client);
            client.StartWork();
            telepasu.log(ModuleName + "Client disconnected...");
            ConnectedUsers--;
            telepasu.log(ModuleName + "Users connected: " + ConnectedUsers);
        }
    }
}
