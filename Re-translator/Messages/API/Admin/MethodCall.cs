﻿using Proxy.ServerEntities;

namespace Proxy.Messages.API.Admin
{
    class MethodCall: ServerMessage
    {
        public string Action;
        protected MethodCall()
        {
            Tag = "Inner Calls";
        }

    }
}