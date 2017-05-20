using System.Runtime.CompilerServices;
using Proxy.Engine;
using Proxy.ServerEntities;

namespace Proxy.Messages.API.Admin
{
    class MethodCall: ServerMessage
    {
        public string Action;
        protected MethodCall()
        {
            Tag = NativeModulesTags.INNER_CALLS;
        }
    }
}
