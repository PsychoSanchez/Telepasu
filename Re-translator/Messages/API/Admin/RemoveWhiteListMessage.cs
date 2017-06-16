using Proxy.ServerEntities;

namespace Proxy.Messages.API.Admin
{
    internal class RemoveWhiteListMessage
    {
        public string Action = "Remove White List";
        public string Address;
    }
}
