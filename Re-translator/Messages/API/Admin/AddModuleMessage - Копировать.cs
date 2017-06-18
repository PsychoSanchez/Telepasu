using Proxy.ServerEntities;

namespace Proxy.Messages.API.Admin
{
    internal class AddUserMessage
    {
        public string Username;
        public string Password;
        public string Role;
    }
}
