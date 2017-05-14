using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities.Messages
{
    class RTCPSentEvent : AsteriskMessage
    {
        public RTCPSentEvent(string _message) : base(_message)
        {
            Channel = Helper.GetValue(_message, "Channel: ");
            CallerIDNum = Helper.GetValue(_message, "CallerIDNum: ");
            CallerIDName = Helper.GetValue(_message, "CallerIDName: ");
            ChannelState = Helper.GetValue(_message, "ChannelState: ");
            ChannelStateDesc = Helper.GetValue(_message, "ChannelStateDesc: ");
            Context = Helper.GetValue(_message, "Context: ");
            ConnectedLineNum = Helper.GetValue(_message, "ConnectedLineNum: ");
            ConnectedLineName = Helper.GetValue(_message, "ConnectedLineName: ");
            Uniqueid = Helper.GetValue(_message, "Uniqueid: ");
            Linkedid = Helper.GetValue(_message, "Linkedid: ");
        }

        public string Channel { get; set; }
        public string ChannelState { get; set; }
        public string ChannelStateDesc { get; set; }
        public string CallerIDNum { get; set; }
        public string CallerIDName { get; set; }
        public string Context { get; set; }
        public string ConnectedLineNum { get; set; }
        public string ConnectedLineName { get; set; }
        public string Uniqueid { get; set; }
        public string Linkedid { get; set; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
