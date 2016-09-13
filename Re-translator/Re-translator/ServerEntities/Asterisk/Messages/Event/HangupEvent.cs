using System;
using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    class HangupEvent : AsteriskMessage
    {
        public HangupEvent(string _message) : base(_message)
        {
            Channel = Helper.GetParameterValue(_message, "Channel: ");
            ConnectedLineNum = Helper.GetParameterValue(_message, "ConnectedLineNum: ");
            ConnectedLineName = Helper.GetParameterValue(_message, "ConnectedLineName: ");
            Cause = Helper.GetParameterValue(_message, "Cause: ");
            Uniqueid = Helper.GetParameterValue(_message, "Uniqueid: ");
            CallerIDNum = Helper.GetParameterValue(_message, "CallerIDNum: ");
            CallerIDName = Helper.GetParameterValue(_message, "CallerIDName: ");
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
