using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities.Messages
{
    class UnholdEvent : AsteriskMessage
    {
        public UnholdEvent(string _message) : base(_message)
        {
            DialStatus = "UNHOLD";
            ChannelID = Helper.GetParameterValue(_message, "Channel: ");
            CallerIDNum = Helper.GetParameterValue(_message, "CallerIDNum: ");
            CallerIDName = Helper.GetParameterValue(_message, "CallerIDName: ");
            ConnectedLineNum = Helper.GetParameterValue(_message, "ConnectedLineNum: ");
            ConnectedLineName = Helper.GetParameterValue(_message, "ConnectedLineName: ");
            Uniqueid = Helper.GetParameterValue(_message, "Uniqueid: ");
            Uniqueid2 = Helper.GetParameterValue(_message, "Linkedid: ");
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
