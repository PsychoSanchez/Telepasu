using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities.Messages
{
    class SIPShowPeerEvent : AsteriskMessage
    {
        public SIPShowPeerEvent(string _message) : base(_message)
        {
            Channeltype = Helper.GetValue(_message, "Channeltype: ");
            ObjectName = Helper.GetValue(_message, "ObjectName: ");
            Context = Helper.GetValue(_message, "Context: ");
            VoiceMailbox = Helper.GetValue(_message, "VoiceMailbox: ");
            Callerid = Helper.GetValue(_message, "Callerid: ");
            AddressIP = Helper.GetValue(_message, "Address-IP: ");
            AddressPort = Helper.GetValue(_message, "Address-Port: ");
            Status = Helper.GetValue(_message, "Status: ");
            ActionID = Helper.GetValue(_message, "ActionID: ");
        }

        public string Channeltype { get; }
        public string ObjectName { get; }
        public string Context { get; }
        public string VoiceMailbox { get; }
        public string Callerid { get; }
        public string AddressIP { get; }
        public string AddressPort { get; }
        public string Status { get; }
        public string ActionID { get; private set; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}

