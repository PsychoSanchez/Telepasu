using Proxy.ServerEntities;

namespace Proxy.Messages.API.Admin
{
    internal class GetModulesMessage
    {
        public string Action = "Get Modules List";
        public object Modules;
    }
}
