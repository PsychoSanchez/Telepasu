using System;
using System.Threading;
using Proxy;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server serv = new Server();
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
                    case "stop":
                        serv.DisconnectAll();
                        break;
                    case "init":
                        serv.init();
                        break;
                    case "start":
                    case "start server":
                        serv.Start();
                        break;
                    case "connect":
                    case "connect asterisk":
                        serv.ConnectAsterisk("bcti", "1488", "127.0.0.1", 8001);
                        break;
                    case "restart":
                        break;
                    case "exit":
                        exit = true;
                        Console.WriteLine("#Application is going to close now");
                        break;
                    default:
                        break;
                }
            }
            Thread.Sleep(5000);
        }
    }
}