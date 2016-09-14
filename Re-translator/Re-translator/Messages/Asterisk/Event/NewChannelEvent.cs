using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities.Messages
{
    class NewChannelEvent : AsteriskMessage
    {

        public NewChannelEvent(string _message) : base(_message)
        {
            ChannelID = Helper.GetValue(base._message, "Channel: ");
            Uniqueid = Helper.GetValue(base._message, "Uniqueid: ");
            CallerIDNum = Helper.GetValue(base._message, "CallerIDNum: ");
            CallerIDName = Helper.GetValue(base._message, "CallerIDName: ");
            Context = Helper.GetValue(base._message, "Context: ");
            ConnectedLineName = Helper.GetValue(base._message, "ConnectedLineName: ");
            ConnectedLineNum = Helper.GetValue(base._message, "ConnectedLineNum: ");
            Status = Helper.GetValue(base._message, "ChannelState: ");
            ChannelStateDesc = Helper.GetValue(base._message, "ChannelStateDesc: ");
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
        //public NewChannelEvent(ChannelData _channel)
        //{
        //    Channel = _channel;
        //    RaiseNewchannelEvent(this);
        //}
        //public NewChannelEvent(string message)
        //{
        //    Channel.ChannelID = Helper.GetValue(message, "Channel: ");
        //    Channel.Uniqueid = Helper.GetValue(message, "Uniqueid: ");
        //    Channel.CallerIDNum = Helper.GetValue(message, "CallerIDNum: ");
        //    Channel.CallerIDName = Helper.GetValue(message, "CallerIDName: ");
        //    Channel.Context = Helper.GetValue(message, "Context: ");
        //    Channel.ConnectedLineName = Helper.GetValue(message, "ConnectedLineName: ");
        //    Channel.ConnectedLineNum = Helper.GetValue(message, "ConnectedLineNum: ");
        //    Channel.Status = Helper.GetValue(message, "ChannelState: ");
        //    Channel.ChannelStateDesc = Helper.GetValue(message, "ChannelStateDesc: ");
        //    RaiseNewchannelEvent(this);
        //}
    }
}
