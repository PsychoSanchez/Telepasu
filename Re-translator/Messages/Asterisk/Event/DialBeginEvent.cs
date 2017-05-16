using System;
using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    class DialBeginEvent : AsteriskMessage
    {
        public DialBeginEvent(string message) : base(message)
        {
            ChannelID = Helper.GetValue(message, "Channel: ");
            if (string.IsNullOrEmpty(ChannelID))
                return;

            DestinationID = Helper.GetValue(message, "Destination: ");
            if (string.IsNullOrEmpty(DestinationID))
            {
                DestinationID = Helper.GetValue(message, "DestChannel: ");
            }

            CallerIDNum = Helper.GetValue(message, "CallerIDNum: ");
            CallerIDName = Helper.GetValue(message, "CallerIDName: ");
            ConnectedLineNum = Helper.GetValue(message, "ConnectedLineNum: ");

            if (string.IsNullOrEmpty(ConnectedLineNum) || ConnectedLineNum == "<unknown>")
                ConnectedLineNum = Helper.GetValue(message, "DestCallerIDNum: ");
            if (ConnectedLineNum == "<unknown>")
            {
                ConnectedLineNum = null;
            }

            ConnectedLineName = Helper.GetValue(message, "ConnectedLineName: ");
            if (string.IsNullOrEmpty(ConnectedLineName) || ConnectedLineName == "<unknown>")
                ConnectedLineName = Helper.GetValue(message, "DestCallerIDName: ");
            if (ConnectedLineName == "<unknown>")
            {
                ConnectedLineName = null;
            }

            Uniqueid = Helper.GetValue(message, "Uniqueid: ");
            if (string.IsNullOrEmpty(Uniqueid))
                Uniqueid = Helper.GetValue(message, "DestLinkedid: ");
            if (string.IsNullOrEmpty(Uniqueid))
                Uniqueid = Helper.GetValue(message, "UniqueID: ");

            Uniqueid2 = Helper.GetValue(message, "DestUniqueid: ");
            if (string.IsNullOrEmpty(Uniqueid2))
                Uniqueid2 = Helper.GetValue(message, "DestUniqueID: ");

            Dialstring = Helper.GetValue(message, "Dialstring: ");
            if (string.IsNullOrEmpty(Dialstring))
                Dialstring = Helper.GetValue(message, "8: ");
        //    currentstatus = DialData.Dialstat.DialBegin;
        //    DialEventArgs e = new DialEventArgs(dial);
        //    RaiseDialEvent(e);
        }

        public string CallerIDName { get; private set; }
        public string CallerIDNum { get; private set; }
        public string ChannelID { get; private set; }
        public string ConnectedLineName { get; private set; }
        public string ConnectedLineNum { get; private set; }
        public string DestinationID { get; private set; }
        public string Dialstring { get; private set; }
        public string Uniqueid { get; private set; }
        public string Uniqueid2 { get; private set; }

        //public DialBeginEvent(string message)
        //{

        //}
        //public DialBeginEvent(ChannelData channel, bool ReverseInformation)
        //{
        //    if (ReverseInformation)
        //        dial = Helper.ChannelDataToDialRev11(channel);
        //    else
        //    {
        //        dial = Helper.ChannelDataToDial(channel);
        //    }
        //    currentstatus = DialData.Dialstat.DialBegin;
        //    DialEventArgs e = new DialEventArgs(dial);
        //    RaiseDialEvent(e);
        //}

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
    //public class ConversationBeginEvent : EventManager
    //{
    //    DialData dial = new DialData();
    //    public ConversationBeginEvent(DialData dialinfo)
    //    {
    //        dial = (DialData)dialinfo.Clone();
    //        currentstatus = DialData.Dialstat.ConversationBegin;
    //        DialEventArgs e = new DialEventArgs(dial);
    //        RaiseDialEvent(e);
    //    }
    //}
    public class DialBegin11 : AsteriskMessage
    {
        public DialBegin11(string message) : base(message)
        {
            ChannelID = Helper.GetValue(message, "Channel: ");
            if (string.IsNullOrEmpty(ChannelID))
                return;

            DestinationID = Helper.GetValue(message, "Destination: ");
            if (string.IsNullOrEmpty(DestinationID))
            {
                DestinationID = Helper.GetValue(message, "DestChannel: ");
            }

            CallerIDNum = Helper.GetValue(message, "CallerIDNum: ");
            CallerIDName = Helper.GetValue(message, "CallerIDName: ");
            ConnectedLineNum = Helper.GetValue(message, "ConnectedLineNum: ");

            if (string.IsNullOrEmpty(ConnectedLineNum) || ConnectedLineNum == "<unknown>")
                ConnectedLineNum = Helper.GetValue(message, "DestCallerIDNum: ");
            if (ConnectedLineNum == "<unknown>")
            {
                ConnectedLineNum = null;
            }

            ConnectedLineName = Helper.GetValue(message, "ConnectedLineName: ");
            if (string.IsNullOrEmpty(ConnectedLineName) || ConnectedLineName == "<unknown>")
                ConnectedLineName = Helper.GetValue(message, "DestCallerIDName: ");
            if (ConnectedLineName == "<unknown>")
            {
                ConnectedLineName = null;
            }

            Uniqueid = Helper.GetValue(message, "Uniqueid: ");
            if (string.IsNullOrEmpty(Uniqueid))
                Uniqueid = Helper.GetValue(message, "DestLinkedid: ");
            if (string.IsNullOrEmpty(Uniqueid))
                Uniqueid = Helper.GetValue(message, "UniqueID: ");

            Uniqueid2 = Helper.GetValue(message, "DestUniqueid: ");
            if (string.IsNullOrEmpty(Uniqueid2))
                Uniqueid2 = Helper.GetValue(message, "DestUniqueID: ");

            Dialstring = Helper.GetValue(message, "Dialstring: ");
            if (string.IsNullOrEmpty(Dialstring))
                Dialstring = Helper.GetValue(message, "8: ");
            //    currentstatus = DialData.Dialstat.DialBegin11;
            //    DialEventArgs e = new DialEventArgs(dial);
            //    RaiseDialEvent(e);
        }
        public string CallerIDName { get; private set; }
        public string CallerIDNum { get; private set; }
        public string ChannelID { get; private set; }
        public string ConnectedLineName { get; private set; }
        public string ConnectedLineNum { get; private set; }
        public string DestinationID { get; private set; }
        public string Dialstring { get; private set; }
        public string Uniqueid { get; private set; }
        public string Uniqueid2 { get; private set; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
