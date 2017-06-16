using Proxy.ServerEntities;

namespace Proxy.Messages.API.Admin
{
    internal class GetUsersOnlineMessage
    {
        public string Action = "Get Users Count";
        public int Count;
    }
}
