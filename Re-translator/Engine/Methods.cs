using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Proxy.ServerEntities;
using Proxy.ServerEntities.Application;
using Proxy.ServerEntities.Module.NativeModule;

namespace Proxy.Engine
{
    struct ConnectionData
    {
        public string Ip;
        public int Port;
        public string Username;
        public string Password;

        public ConnectionData(int port, string ip, string password, string username)
        {
            Port = port;
            Ip = ip;
            Password = password;
            Username = username;
        }
    }
    class Methods
    {
        public static void Subscribe(string tag, string id)
        {
            
        }

        public static void Unsubscribe(string tag, string id)
        {
            
        }

        public static void RequestModule(string ip, int port)
        {
            // TODO: Сгенерировать уникальный идентификатор и отправить модулю с запросом на подтверждение
        }

        public static void RegisterModule()
        {
            // TODO: Зарегистрировать модуль во внутренней базе данных
            // Добавить автоподписку на сообщения по своему Uid
            // Добавить название модуля 
            // Добавить IP адрес и порт 
            // Записать данные в локальную базу
        }

        public static string[] GetModulesList()
        {
            // TODO: return массив uid
            return new string[10];
        }

        public static string[] GetAppList()
        {    
            return new string[10];
        }

        public static async void ConnectNativeModule(string type, ConnectionData data)
        {
            switch (type)
            {
                case "Asterisk":
                    Socket socket = GetSocket();
                    try
                    {
                        IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(data.Ip), Convert.ToInt32(data.Port));
                        socket.Connect(endpoint);
                        AsteriskEntity asterisk = new AsteriskEntity(socket);
                        if ( await asterisk.Login(data.Username, data.Password))
                        {
                            telepasu.log("#Asterisk connected...");
                            ProxyEngine.MailPost.AddNativeModule("AsteriskServer1", asterisk);
                            ThreadPool.QueueUserWorkItem(AsteriskThread, asterisk);
                        }

                        telepasu.log("#Failed to connect asterisk...");
                    }
                    catch (SocketException e)
                    {
                        if (e.SocketErrorCode != SocketError.TimedOut) return;

                        telepasu.log("Сервер недоступен");
                        throw new Exception("Сервер недоступен");
                    }
                    break;
                case "Postgres":
                    break;
                default:
                    break;
            }
        }

        private static void AsteriskThread(object state)
        {
            EntityManager asterisk = (EntityManager)state;
            asterisk.StartWork();
        }

        private static Socket GetSocket()
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
    }
}
