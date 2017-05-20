using Proxy.Messages.API.Admin;
using Proxy.ServerEntities.Application;

namespace Proxy.Messages.API.SystemCalls
{
    internal class LocalDbLoginMessage : MethodCall
    {
        public string Login;
        public string Secret;
        public string Role;
        public GuestEntity Sender;
    }
}
