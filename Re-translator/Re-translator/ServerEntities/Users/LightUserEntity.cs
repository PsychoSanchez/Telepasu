﻿using System;
using System.Net.Sockets;

namespace Proxy.ServerEntities.Users
{
    class LightUserEntity : UserManager
    {
        public LightUserEntity(Socket _client) : base(_client)
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
