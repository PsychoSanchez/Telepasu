using System;
using System.Threading;
using Proxy;
using Proxy.Engine;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new ProxyEngine();
            engine.Start();
            SocketServer serv = new SocketServer();
            bool exit = false;
            while (!exit)
            {
                string value = Console.ReadLine();
                if (value == null) continue;
                switch (value.ToLower())
                {
                    case "accept":
                    case "accept client":
                        //serv.AcceptClient();
                        break;
                    case "stop":
                        //serv.DisconnectAll();
                        break;
                    case "init":
                        //serv.Init();
                        break;
                    case "start":
                    case "start server":
                        //serv.Start();
                        break;
                    case "connect db":
                        //serv.ConnectDatabase("bcti", "hjok123", "127.0.0.1", 8001);
                        break;
                    case "connect asterisk":
                        //serv.ConnectAsterisk("mark", "hjok123", "192.168.1.39", 5038);
                        //serv.ConnectAsterisk("admin", "hjok123", "192.168.1.44", 5038);
                        break;
                    case "restart":
                        break;
                    case "exit":
                        engine.Stop();
                        exit = true;
                        telepasu.log("#Application is going to close now");
                        break;
                    default:
                        break;
                }
            }
            Thread.Sleep(2000);
        }
    }
}