using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.ComponentModel;

namespace client
{
    class Program
    {
        public const string LINE_SEPARATOR = "\r\n";
        void Work(Object obj)
        {

        }
        BackgroundWorker reciever;
        Socket socket;
        public Socket GetSocket()
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
        private void initReciever()
        {
            reciever = new BackgroundWorker();
            reciever.DoWork += Listen;
            reciever.WorkerSupportsCancellation = true;
            reciever.RunWorkerAsync();
        }
        private bool SocketConnected()
        {
            return !((socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0)) || !socket.Connected);

            ///Simpler version
            //bool part1 = socket.Poll(1000, SelectMode.SelectRead);
            //bool part2 = (socket.Available == 0);
            //if (part1 && part2)
            //    return false;
            //else
            //    return true;
        }
        private void Listen(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            byte[] recvBuffer = new byte[100000];
            string Response = string.Empty;
            int buffersize;
            do
            {
                Thread.Sleep(15);
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    Console.WriteLine("Asterisk thread disconnected");
                    return;
                }
                buffersize = socket.Receive(recvBuffer);

                Response += Encoding.ASCII.GetString(recvBuffer, 0, buffersize);

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Console.WriteLine(Response);
                Response = string.Empty;
            }
            while (SocketConnected());


        }


        void start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            socket = GetSocket();
            socket.Connect(ep);
            initReciever();
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.start();

            //press any key to exit
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}