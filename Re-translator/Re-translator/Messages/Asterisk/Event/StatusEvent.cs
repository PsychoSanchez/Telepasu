using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities.Messages
{
    class StatusEvent : AsteriskMessage
    {
        public StatusEvent(string _message) : base(_message)
        {
            Channel = Helper.GetValue(_message, "Channel: ");
        }

        private string Channel { get; set; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
