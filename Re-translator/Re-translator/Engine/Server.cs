﻿using Proxy.ServerEntities;
using Proxy.ServerEntities.SQL;
using Proxy.ServerEntities.Users;
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
            DisconnectAll();
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
            Socket socket = GetSocket();
            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ip), Convert.ToInt32(port));
                socket.Connect(endpoint);
                AsteriskEntity asterisk = new AsteriskEntity(socket);
                if (asterisk.Login(username, password))
                {
                    Mail.AddAsterisk(asterisk);
                    ThreadPool.QueueUserWorkItem(AsteriskThread, Mail.Asterisk);
                }
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.TimedOut)
                {
                    telepasu.log("Сервер недоступен");
                    throw new Exception("Сервер недоступен");
                }
                return false;
            }
            return true;
        }
        void AsteriskThread(object obj)
        {
            UserManager asterisk = (UserManager)obj;
            asterisk.StartWork();
            Console.WriteLine("ASterisk thread stoped...");
        }
        public bool ConnectDatabase(string username, string password, string ip, int port)
        {
            IDB db = new FakeDB();
            if (db.ConnectDB("123", "123", "123", "123"))
            {
                Mail.AddDB(db);
                Console.WriteLine("#Database connected...");
                return true;
            }
            Console.WriteLine("#Failed to connect database...");
            return false;
        }
        private Socket GetSocket()
        {
            Socket newsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Don't allow another socket to bind to this port.
            newsocket.ExclusiveAddressUse = true;
            // Timeout 3 seconds
            newsocket.SendTimeout = 30000;
            newsocket.ReceiveTimeout = 70000;
            // Set the Time To Live (TTL) to 42 router hops.
            newsocket.Ttl = 42;
            return newsocket;
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
            UserManager ServerUser = new GuestEntity(listener.EndAcceptSocket(ar), 5000);
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
            UserManager newEntity = temp.StartAutorization();
            if (newEntity == null)
            {
                Console.WriteLine("Client kicked...");
                return;
            }
            Console.WriteLine("Client accepted...");
            Mail.AddUser(newEntity);
            newEntity.StartWork();
            Console.WriteLine("Client thread stoped...");
        }
        public void DisconnectAll()
        {
            foreach (var user in Mail.GetUsers())
            {
                user.Shutdown();
            }
            Mail.Clear();
        }
    }
}