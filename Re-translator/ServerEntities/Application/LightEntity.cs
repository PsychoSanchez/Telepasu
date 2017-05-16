using System;
using System.Net.Sockets;

namespace Proxy.ServerEntities.Application
{
    class LightEntity : EntityManager
    {
        public LightEntity(SocketMail mail) : base(mail)
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

        protected override void WorkAction()
        {
            throw new NotImplementedException();
        }
    }
}
