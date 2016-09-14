using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities.Messages
{
    class DialEndEvent : AsteriskMessage
    {
        public DialEndEvent(string _message):base(_message)
        {
            if (Helper.GetValue(_message, "Event: ") == "DialEnd")
            {
                DialStatus = Helper.GetValue(_message, "DialStatus: ");

                ChannelID = Helper.GetValue(_message, "Channel: ");
                DestinationID = Helper.GetValue(_message, "DestChannel: ");

                CallerIDNum = Helper.GetValue(_message, "CallerIDNum: ");
                CallerIDName = Helper.GetValue(_message, "CallerIDName: ");

                ConnectedLineNum = Helper.GetValue(_message, "ConnectedLineNum: ");
                if (string.IsNullOrEmpty(ConnectedLineNum) || ConnectedLineNum == "<unknown>")
                    ConnectedLineNum = Helper.GetValue(_message, "DestCallerIDNum: ");

                ConnectedLineName = Helper.GetValue(_message, "ConnectedLineName: ");
                if (string.IsNullOrEmpty(ConnectedLineName) || ConnectedLineName == "<unknown>")
                    ConnectedLineName = Helper.GetValue(_message, "DestCallerIDName: ");

                Uniqueid = Helper.GetValue(_message, "Uniqueid: ");
                if (string.IsNullOrEmpty(Uniqueid))
                    Uniqueid = Helper.GetValue(_message, "DestLinkedid: ");
                Uniqueid2 = Helper.GetValue(_message, "DestUniqueid: ");
            }
            else
            {
                DialStatus = Helper.GetValue(_message, "DialStatus: ");
                ChannelID = Helper.GetValue(_message, "Channel: ");
                Uniqueid = Helper.GetValue(_message, "UniqueID: ");
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

