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
}
