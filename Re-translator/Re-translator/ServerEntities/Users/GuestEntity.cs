using System.Net.Sockets;

namespace Proxy.ServerEntities.Users
{
    class GuestEntity
    {
        public Socket client;
        public GuestEntity(Socket _client)
        {
            this.client = _client;
        }

        public UserManager StartAutorization()
        {
            var huinya = "nyaaa";
            switch (huinya)
            {
                case "Hello kek":
                    return new AdminUser(this.client);
                    break;
                default: 
                    return new AdminUser(this.client);
                    break;
            } 
        }
    }
}
