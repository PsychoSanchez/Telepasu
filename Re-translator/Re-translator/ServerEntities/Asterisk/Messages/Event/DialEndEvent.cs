using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities.Messages
{
    class DialEndEvent : AsteriskMessage
    {
        public DialEndEvent(string _message):base(_message)
        {
            if (Helper.GetParameterValue(_message, "Event: ") == "DialEnd")
            {
                DialStatus = Helper.GetParameterValue(_message, "DialStatus: ");

                ChannelID = Helper.GetParameterValue(_message, "Channel: ");
                DestinationID = Helper.GetParameterValue(_message, "DestChannel: ");

                CallerIDNum = Helper.GetParameterValue(_message, "CallerIDNum: ");
                CallerIDName = Helper.GetParameterValue(_message, "CallerIDName: ");

                ConnectedLineNum = Helper.GetParameterValue(_message, "ConnectedLineNum: ");
                if (string.IsNullOrEmpty(ConnectedLineNum) || ConnectedLineNum == "<unknown>")
                    ConnectedLineNum = Helper.GetParameterValue(_message, "DestCallerIDNum: ");

                ConnectedLineName = Helper.GetParameterValue(_message, "ConnectedLineName: ");
                if (string.IsNullOrEmpty(ConnectedLineName) || ConnectedLineName == "<unknown>")
                    ConnectedLineName = Helper.GetParameterValue(_message, "DestCallerIDName: ");

                Uniqueid = Helper.GetParameterValue(_message, "Uniqueid: ");
                if (string.IsNullOrEmpty(Uniqueid))
                    Uniqueid = Helper.GetParameterValue(_message, "DestLinkedid: ");
                Uniqueid2 = Helper.GetParameterValue(_message, "DestUniqueid: ");
            }
            else
            {
                DialStatus = Helper.GetParameterValue(_message, "DialStatus: ");
                ChannelID = Helper.GetParameterValue(_message, "Channel: ");
                Uniqueid = Helper.GetParameterValue(_message, "UniqueID: ");
                //currentstatus = DialData.Dialstat.ConversationEnd;
           }
        }
        //public DialEndEvent(ChannelData channel)
        //{
        //    dial = Helper.ChannelDataToDial(channel);
        //    dial.DialStatus = "END";
        //    dial.currentstatus = DialData.Dialstat.ConversationEnd;
        //    DialEventArgs e = new DialEventArgs(dial);
        //    RaiseDialEvent(e);
        //}
        //public DialEndEvent(DialData dialinfo)
        //{
        //    dial = dialinfo;
        //    if (string.IsNullOrEmpty(dialinfo.DialStatus))
        //        dial.DialStatus = "END";
        //    dial.currentstatus = DialData.Dialstat.ConversationEnd;
        //    DialEventArgs e = new DialEventArgs(dial);
        //    RaiseDialEvent(e);
        //}

        public string CallerIDName { get; private set; }
        public string CallerIDNum { get; private set; }
        public string ChannelID { get; private set; }
        public string ConnectedLineName { get; private set; }
        public string ConnectedLineNum { get; private set; }
        public string DestinationID { get; private set; }
        public string DialStatus { get; private set; }
        public string Uniqueid { get; private set; }
        public string Uniqueid2 { get; private set; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}

