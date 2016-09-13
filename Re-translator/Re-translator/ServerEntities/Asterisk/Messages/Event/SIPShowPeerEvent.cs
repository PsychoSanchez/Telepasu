using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities.Messages
{
    class SIPShowPeerEvent : AsteriskMessage
    {
        public SIPShowPeerEvent(string _message) : base(_message)
        {
            Channeltype = Helper.GetParameterValue(_message, "Channeltype: ");
            ObjectName = Helper.GetParameterValue(_message, "ObjectName: ");
            Context = Helper.GetParameterValue(_message, "Context: ");
            VoiceMailbox = Helper.GetParameterValue(_message, "VoiceMailbox: ");
            Callerid = Helper.GetParameterValue(_message, "Callerid: ");
            AddressIP = Helper.GetParameterValue(_message, "Address-IP: ");
            AddressPort = Helper.GetParameterValue(_message, "Address-Port: ");
            Status = Helper.GetParameterValue(_message, "Status: ");
            ActionID = Helper.GetParameterValue(_message, "ActionID: ");
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

