using System;
using System.Net.Sockets;
using System.Threading;

namespace Proxy.ServerEntities.Users
{
    class GuestEntity : UserManager
    {
        public Socket client;

        public GuestEntity(Socket _client, int timeout) : base(_client, timeout)
        {
            this.client = _client;
        }
        public UserManager StartAutorization()
        {
            Thread.Sleep(6000);
            var hui = "nyaaa";
            switch (hui)
            {
                case "Hello kek":
                    return new AdminUser(this.client);
                    break;
                default: 
                    return new AdminUser(this.client);
                    break;
            } 
        }
        protected override void TimeOut(object sender, EventArgs e)
        {
            Console.WriteLine("guest timedOut");
        }
        protected override void Disconnected(object sender, MessageArgs e)
        {
            Console.WriteLine("guest disconnected");
            return;
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            Console.WriteLine("guest message recieved");
            return;
        }

        protected override void WorkCycle()
        {
            return;
        }
    }
}
