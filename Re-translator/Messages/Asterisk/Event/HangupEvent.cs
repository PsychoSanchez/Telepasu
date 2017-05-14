using System;
using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    class HangupEvent : AsteriskMessage
    {
        public HangupEvent(string _message) : base(_message)
        {
            Channel = Helper.GetValue(_message, "Channel: ");
            ConnectedLineNum = Helper.GetValue(_message, "ConnectedLineNum: ");
            ConnectedLineName = Helper.GetValue(_message, "ConnectedLineName: ");
            Cause = Helper.GetValue(_message, "Cause: ");
            Uniqueid = Helper.GetValue(_message, "Uniqueid: ");
            CallerIDNum = Helper.GetValue(_message, "CallerIDNum: ");
            CallerIDName = Helper.GetValue(_message, "CallerIDName: ");
        }

        public string Channel { get; private set; }
        public string Cause { get; private set; }
        public string Uniqueid { get; private set; }
        public string CallerIDNum { get; private set; }
        public string CallerIDName { get; private set; }
        public string ConnectedLineNum { get; private set; }
        public string ConnectedLineName { get; private set; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
