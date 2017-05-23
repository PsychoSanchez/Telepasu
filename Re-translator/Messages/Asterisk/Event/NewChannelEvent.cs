using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities.Messages
{
    class NewChannelEvent : AsteriskMessage
    {

        public NewChannelEvent(string message) : base(message)
        {
            ChannelID = Helper.GetValue(base.Message.ToString(), "Channel: ");
            Uniqueid = Helper.GetValue(base.Message.ToString(), "Uniqueid: ");
            CallerIDNum = Helper.GetValue(base.Message.ToString(), "CallerIDNum: ");
            CallerIDName = Helper.GetValue(base.Message.ToString(), "CallerIDName: ");
            Context = Helper.GetValue(base.Message.ToString(), "Context: ");
            ConnectedLineName = Helper.GetValue(base.Message.ToString(), "ConnectedLineName: ");
            ConnectedLineNum = Helper.GetValue(base.Message.ToString(), "ConnectedLineNum: ");
            Status = Helper.GetValue(base.Message.ToString(), "ChannelState: ");
            ChannelStateDesc = Helper.GetValue(base.Message.ToString(), "ChannelStateDesc: ");
        }

        public string CallerIDName { get; private set; }
        public string CallerIDNum { get; private set; }
        public string ChannelID { get; private set; }
        public string ChannelStateDesc { get; private set; }
        public string ConnectedLineName { get; private set; }
        public string ConnectedLineNum { get; private set; }
        public string Context { get; private set; }
        public string Status { get; private set; }
        public string Uniqueid { get; private set; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
