using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities.Messages
{
    class NewChannelEvent : AsteriskMessage
    {

        public NewChannelEvent(string _message) : base(_message)
        {
            ChannelID = Helper.GetParameterValue(base._message, "Channel: ");
            Uniqueid = Helper.GetParameterValue(base._message, "Uniqueid: ");
            CallerIDNum = Helper.GetParameterValue(base._message, "CallerIDNum: ");
            CallerIDName = Helper.GetParameterValue(base._message, "CallerIDName: ");
            Context = Helper.GetParameterValue(base._message, "Context: ");
            ConnectedLineName = Helper.GetParameterValue(base._message, "ConnectedLineName: ");
            ConnectedLineNum = Helper.GetParameterValue(base._message, "ConnectedLineNum: ");
            Status = Helper.GetParameterValue(base._message, "ChannelState: ");
            ChannelStateDesc = Helper.GetParameterValue(base._message, "ChannelStateDesc: ");
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
        //    Channel.ChannelID = Helper.GetParameterValue(message, "Channel: ");
        //    Channel.Uniqueid = Helper.GetParameterValue(message, "Uniqueid: ");
        //    Channel.CallerIDNum = Helper.GetParameterValue(message, "CallerIDNum: ");
        //    Channel.CallerIDName = Helper.GetParameterValue(message, "CallerIDName: ");
        //    Channel.Context = Helper.GetParameterValue(message, "Context: ");
        //    Channel.ConnectedLineName = Helper.GetParameterValue(message, "ConnectedLineName: ");
        //    Channel.ConnectedLineNum = Helper.GetParameterValue(message, "ConnectedLineNum: ");
        //    Channel.Status = Helper.GetParameterValue(message, "ChannelState: ");
        //    Channel.ChannelStateDesc = Helper.GetParameterValue(message, "ChannelStateDesc: ");
        //    RaiseNewchannelEvent(this);
        //}
    }
}
