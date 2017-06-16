using Proxy.Engine;
using Proxy.ServerEntities;

namespace Proxy.Messages.API.Admin
{
    public class AddModuleMethod: MethodCall
    {
        public string Type;
        public string Username;
        public string Pwd;
        public string Ip;
        public int Port;
        public AddModuleMethod(EntityManager sender): base(sender)
        {
        }
    }
}
