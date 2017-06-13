using Proxy.Engine;
using Proxy.Messages.API.Admin;
using Proxy.ServerEntities;
using Proxy.ServerEntities.Application;

namespace Proxy.Messages.API.SystemCalls
{
    internal class LocalDbLoginMessage : MethodCall
    {
        public string Login;
        public string Challenge;
        public string Secret;
        public string Role;

        public LocalDbLoginMessage(EntityManager sender) : base(sender)
        {
        }
    }
}
