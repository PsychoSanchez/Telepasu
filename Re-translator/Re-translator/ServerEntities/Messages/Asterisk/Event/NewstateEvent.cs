using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities.Messages
{
    class NewstateEvent : AsteriskMessage
    {
        public NewstateEvent(string _message) : base(_message)
        {
            Channel = Helper.GetParameterValue(_message, "Channel: ");
            CallerIDNum = Helper.GetParameterValue(_message, "CallerIDNum: ");
            CallerIDName = Helper.GetParameterValue(_message, "CallerIDName: ");
            ChannelState = Helper.GetParameterValue(_message, "ChannelState: ");
            ChannelStateDesc = Helper.GetParameterValue(_message, "ChannelStateDesc: ");
            Context = Helper.GetParameterValue(_message, "Context: ");
            ConnectedLineNum = Helper.GetParameterValue(_message, "ConnectedLineNum: ");
            ConnectedLineName = Helper.GetParameterValue(_message, "ConnectedLineName: ");
        }

        public string Channel { get; set; }
        public string ChannelState { get; set; }
        public string ChannelStateDesc { get; set; }
        public string CallerIDNum { get; set; }
        public string CallerIDName { get; set; }
        public string Context { get; set; }
        public string ConnectedLineNum { get; set; }
        public string ConnectedLineName { get; set; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
