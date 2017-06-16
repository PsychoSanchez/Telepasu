using System.Runtime.CompilerServices;
using Proxy.Engine;
using Proxy.ServerEntities;

namespace Proxy.Messages.API.Admin
{
    public class MethodCall: ServerMessage
    {
        public string Action;
        public EntityManager Sender;

        public MethodCall(EntityManager sender)
        {
            Sender = sender;
            Tag = NativeModulesTags.INNER_CALLS;
        }
    }
}
