using Proxy.Engine;
using Proxy.Messages.API.Admin;
using Proxy.ServerEntities;

namespace Proxy.Messages.API.SystemCalls
{
    internal class SubscribeMethod : MethodCall
    {
        public string Id;
        public string SubscribeTag;

        public SubscribeMethod(EntityManager sender) : base(sender)
        {
        }
    }
}
