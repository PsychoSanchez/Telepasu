using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Concurrent;

namespace server
{
    public class Server
    {
        public static ConcurrentDictionary<string, ConcurrentQueue<string>> dick = new ConcurrentDictionary<string, ConcurrentQueue<string>>();
        ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

        void ProcessIncomingData(object obj)
        {
            TcpClient client = (TcpClient)obj;
            StringBuilder sb = new StringBuilder();

            using (NetworkStream stream = client.GetStream())
            {
                int i;
                while ((i = stream.ReadByte()) != 0)
                {
                    sb.Append((char)i);
                }

                string reply = "ack: " + sb.ToString() + '\0';
                //stream.Write(Encoding.ASCII.GetBytes(reply), 0, reply.Length);
            }
            Console.WriteLine(sb.ToString());
        }

        void ProcessIncomingConnection(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(ar);

            ThreadPool.QueueUserWorkItem(ProcessIncomingData, client);
            tcpClientConnected.Set();
        }

        public void Start()
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            TcpListener listener = new TcpListener(endpoint);
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