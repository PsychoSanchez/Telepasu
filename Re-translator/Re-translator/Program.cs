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
    public class Server
    {
        int ThreadCount = 0;
        public static List<ServerEntity> ClientList = new List<ServerEntity>();
        public static ConcurrentDictionary<string, ConcurrentQueue<string>> dick = new ConcurrentDictionary<string, ConcurrentQueue<string>>();
        ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

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

        public void StartProxy()
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            //IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, Int32.Parse(tbPortNumber.Text));
            TcpListener listener = new TcpListener(endpoint);

            Console.WriteLine("Server Started... Waiting for connection...");
            listener.Start();

            while (true)
            {
                tcpClientConnected.Reset();
                listener.BeginAcceptTcpClient(new AsyncCallback(ProcessIncomingConnection), listener);
                tcpClientConnected.WaitOne();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Server s = new Server();
            s.StartProxy();
        }
    }
}