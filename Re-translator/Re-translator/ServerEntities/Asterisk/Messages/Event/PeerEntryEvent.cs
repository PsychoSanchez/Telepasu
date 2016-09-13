using System;
using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    class PeerEntryEvent : AsteriskMessage
    {
        public PeerEntryEvent(string _message) : base(_message)
        {
            Number = Helper.GetParameterValue(_message, "ObjectName: ");
            Protocol = Helper.GetParameterValue(_message, "Channeltype: ");
            Context = Helper.GetParameterValue(_message, "Context: ");
            IP = Helper.GetParameterValue(_message, "IPaddress: ");
            if (string.IsNullOrEmpty(IP))
                IP = Helper.GetParameterValue(_message, "Address-IP: ");
            Port = Helper.GetParameterValue(_message, "IPport: ");
            if (string.IsNullOrEmpty(Port))
                Port = Helper.GetParameterValue(_message, "Address-Port: ");
            Status = Helper.GetParameterValue(_message, "Status: ");
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
