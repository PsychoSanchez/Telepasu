using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Concurrent;
using Re_translator.ServerEntities;

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
        public static ConcurrentDictionary<string, ConcurrentQueue<string>> dick = new ConcurrentDictionary<string, ConcurrentQueue<string>>();
        ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

        ServerEntity StartAuthorization()
        {
            StringBuilder sb = new StringBuilder();

            using (NetworkStream stream = temp.client.GetStream())
            {
                int i;
                while ((i = stream.ReadByte()) != -1)
                {
                    sb.Append((char)i);
                }
                Console.WriteLine(sb.ToString());
                Console.WriteLine("Client accepted");
            }
            return new UserEntity();
        }
        void ProcessIncomingData(object obj)
        {
            StartAuthorization();
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
                //string reply = "ack: " + sb.ToString() + '\0';
                //stream.Write(Encoding.ASCII.GetBytes(reply), 0, reply.Length);
            }
        }


        void ProcessIncomingConnection(IAsyncResult ar)
        {
            ThreadObj temp = new ThreadObj();
            temp.ThreadNUmber = ++ThreadCount;

            TcpListener listener = (TcpListener)ar.AsyncState;
            temp.client = listener.EndAcceptTcpClient(ar);

            //var oipi = ((IPEndPoint)temp.client.Client.RemoteEndPoint).Address.ToString();
            //if(ipTable.Compare(oipi)){
            ThreadPool.QueueUserWorkItem(ProcessIncomingData, temp);
            //}

            tcpClientConnected.Set();
        }

        string ip1 = "192.168.1.10";
        string ip2 = "192.168.11.0";
        string ip3 = "192.168.1.100";
        public void Start()
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            //IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, Int32.Parse(tbPortNumber.Text));
            TcpListener listener = new TcpListener(endpoint);
            int minWorker, minIOC;
            // Get the current settings.
            ThreadPool.GetMinThreads(out minWorker, out minIOC);

            //int MaxThreadsCount = Environment.ProcessorCount * 4;
            //// Установим максимальное количество рабочих потоков
            //ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
            ////ThreadPool.SetMinThreads()
            Console.WriteLine(String.Compare(ip1, ip2));
            Console.WriteLine(String.Compare(ip2, ip3));
            Console.WriteLine(minWorker);
            Console.WriteLine(minIOC);
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
            s.Start();
        }
    }
}