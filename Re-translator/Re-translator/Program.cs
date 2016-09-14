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
                    case "connect db":
                        serv.ConnectDatabase("bcti", "hjok123", "127.0.0.1", 8001);
                        break;
                    case "connect asterisk":
                        serv.ConnectAsterisk("mark", "hjok123", "192.168.1.40", 5038);
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
            Thread.Sleep(2000);
        }
    }
}