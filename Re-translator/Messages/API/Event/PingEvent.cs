using System;
using Proxy.ServerEntities;
using Proxy.Helpers;

namespace Proxy.Messages.API
{
    class PingEvent : ServerMessage
    {
        public string ActionID { get; set; }
        string message;
        public PingEvent()
        {
           
        }
        public override string ToString()
        {
          
            return message;
        }
    }
}
