using System;
using System.Net.Sockets;

namespace Proxy.ServerEntities.Users
{
    class LightUser : UserManager
    {
        public LightUser(SocketMail mail) : base(mail)
        {
        }

        protected override void Disconnected(object sender, MessageArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void WorkCycle()
        {
            throw new NotImplementedException();
        }
    }
}
