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

       
    }
}
