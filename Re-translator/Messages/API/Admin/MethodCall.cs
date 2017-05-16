using Proxy.ServerEntities;

namespace Proxy.Messages.API.Admin
{
    class MethodCall: ServerMessage
    {
        protected MethodCall()
        {
            Tag = "Inner Calls";
        }
    }
}
