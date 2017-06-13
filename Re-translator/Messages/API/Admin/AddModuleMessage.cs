using Proxy.ServerEntities;

namespace Proxy.Messages.API.Admin
{
    internal class AddModuleMessage
    {
        public string Type;
        public string Username;
        public string Pwd;
        public string Ip;
        public int Port;
    }
}
