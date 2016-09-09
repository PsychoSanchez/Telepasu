using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Concurrent;
using Re_translator.ServerEntities;
using System.Collections.Generic;

namespace server
{
    public class ThreadObj
    {
        public TcpClient client;
        public int ThreadNUmber;
    }
    public class Proxy
    {
        IPEndPoint endpoint;
        TcpListener listener;
        int ThreadCount = 0;
        public static List<ServerEntity> ClientList = new List<ServerEntity>();
        public static ConcurrentDictionary<string, ConcurrentQueue<string>> dick = new ConcurrentDictionary<string, ConcurrentQueue<string>>();
        ManualResetEvent tcpClientConnected = new ManualResetEvent(false);
        bool StopProxy = false;

        //ServerEntity StartAuthorization()
        //{
        //    StringBuilder sb = new StringBuilder();

        //    using (NetworkStream stream = temp.client.GetStream())
        //    {
        //        int i;
        //        while ((i = stream.ReadByte()) != -1)
        //        {
        //            sb.Append((char)i);
        //        }
        //        Console.WriteLine(sb.ToString());
        //        Console.WriteLine("Client accepted");
        //    }
        //    return new UserEntity();
        //}

        void ProcessIncomingData(object obj)
        {
            //try
            //{
            //StartAuthorization();
            ///DoWork
            //}
            //catch
            //{
            ///Exception
            //}
            //finally
            //{
            ///Restart Thread
            //}
            ThreadObj temp = (ThreadObj)obj;
            TcpClient client = temp.client;
            var ThreadNumber = temp.ThreadNUmber;

            StringBuilder sb = new StringBuilder();

            using (NetworkStream stream = client.GetStream())
            {
                int i;
                while ((i = stream.ReadByte()) != -1)
                {
                    sb.Append((char)i);
                }
                Console.WriteLine(sb.ToString());
                Console.WriteLine("Client accepted");
                while (true)
                {
                    Console.WriteLine("Thread " + ThreadNumber + " waiting");
                    Thread.Sleep(300);
                }
            }
        }


        void ProcessIncomingConnection(IAsyncResult ar)
        {

            ThreadObj temp = new ThreadObj();
            temp.ThreadNUmber = ++ThreadCount;
            ///Проверять авторизацию до создания нового потока не вариант.
            //Thread.Sleep(3000);
            TcpListener listener = (TcpListener)ar.AsyncState;
            temp.client = listener.EndAcceptTcpClient(ar);

            //Проверка наличия айпи в белом листе
            //var oipi = ((IPEndPoint)temp.client.Client.RemoteEndPoint).Address.ToString();
            //if(ipTable.Compare(oipi)){
            ThreadPool.QueueUserWorkItem(ProcessIncomingData, temp);
            //}

            tcpClientConnected.Set();
        }


        public void initInnerThreads()
        {

        }

        public void Start()
        {
            //IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, Int32.Parse(tbPortNumber.Text));
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

            //// НЕ ЗАБЫТЬ ПЕРЕПИСАТЬ АРХИТЕКТУРУ ПОД CANCELLATION TOKENS
            CancellationTokenSource cts = new CancellationTokenSource();

            for (int i = 0; i < 100; i++)
            {
                ThreadPool.QueueUserWorkItem(s =>
                {
                    CancellationToken token = (CancellationToken)s;
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    Console.WriteLine("Output2");
                    token.WaitHandle.WaitOne(1000);
                }, cts.Token);
            }
            cts.Cancel();
            /////
        }
        /// <summary>
        /// Инициализация подключения к сереверу
        /// </summary>
        public void init()
        {
            endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000)
            listener = new TcpListener(endpoint);
            listener.Start();
            Console.WriteLine("#Server initialized...");
        }
        /// <summary>
        /// Функция посадки клиента
        /// </summary>
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
    }

    class Program
    {
        static void Main(string[] args)
        {
            Proxy serv = new Proxy();
            bool exit = false;
            while (!exit)
            {
                string value = Console.ReadLine();
                switch (value.ToLower())
                {
                    case "accept":
                    case "accept client":
                        serv.AcceptClient();
                        break;
                    case "init":
                    case "start":
                    case "start server":
                        serv.init();
                        break;
                    case "connect asterisk":
                        serv.ConnectAsterisk("mark", "1488", "127.0.0.1", 5038);
                        break;
                    case "restart":
                        break;
                    case "exit":
                        exit = true;
                        Console.WriteLine("#Application is going to close now");
                        break;
                    default:
                        ;
                        break;
                }
            }
            Thread.Sleep(5000);
        }
    }
}