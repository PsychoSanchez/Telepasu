using System;
using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    class PeerEntryEvent : AsteriskMessage
    {
        public PeerEntryEvent(string _message) : base(_message)
        {
            Number = Helper.GetValue(_message, "ObjectName: ");
            Protocol = Helper.GetValue(_message, "Channeltype: ");
            Context = Helper.GetValue(_message, "Context: ");
            IP = Helper.GetValue(_message, "IPaddress: ");
            if (string.IsNullOrEmpty(IP))
                IP = Helper.GetValue(_message, "Address-IP: ");
            Port = Helper.GetValue(_message, "IPport: ");
            if (string.IsNullOrEmpty(Port))
                Port = Helper.GetValue(_message, "Address-Port: ");
            Status = Helper.GetValue(_message, "Status: ");
        }

        public string Context { get; private set; }
        public string IP { get; private set; }
        public string Number { get; private set; }
        public string Port { get; private set; }
        public string Protocol { get; private set; }
        public string Status { get; private set; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
