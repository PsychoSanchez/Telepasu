using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxy.ServerEntities;

namespace Proxy.Messages.API.Admin
{
    internal class AddModuleCommand: MethodCall
    {
        public string Type;
        public string Username;
        public string Pwd;
        public string Ip;
        public int Port;
        public AddModuleCommand(): base()
        {
        }
    }
}
