using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.ServerEntities.UserEntities
{
    class GuestEntity
    {
        public Socket client;
        public GuestEntity(Socket _client)
        {
            this.client = _client;
        }

        public ServerEntity StartAutorization()
        {
            return new AdminEntity(this.client);
        }
    }
}
