using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities.Messages
{
    class UnholdEvent : AsteriskMessage
    {
        public UnholdEvent(string _message) : base(_message)
        {
            DialStatus = "UNHOLD";
            ChannelID = Helper.GetValue(_message, "Channel: ");
            CallerIDNum = Helper.GetValue(_message, "CallerIDNum: ");
            CallerIDName = Helper.GetValue(_message, "CallerIDName: ");
            ConnectedLineNum = Helper.GetValue(_message, "ConnectedLineNum: ");
            ConnectedLineName = Helper.GetValue(_message, "ConnectedLineName: ");
            Uniqueid = Helper.GetValue(_message, "Uniqueid: ");
            Uniqueid2 = Helper.GetValue(_message, "Linkedid: ");
        }

        public string CallerIDName { get; private set; }
        public string CallerIDNum { get; private set; }
        public string ChannelID { get; private set; }
        public string ConnectedLineName { get; private set; }
        public string ConnectedLineNum { get; private set; }
        public string DialStatus { get; private set; }
        public string Uniqueid { get; private set; }
        public string Uniqueid2 { get; private set; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
