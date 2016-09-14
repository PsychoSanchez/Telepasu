using System;
using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    class DialBeginEvent : AsteriskMessage
    {
        public DialBeginEvent(string _message) : base(_message)
        {
            ChannelID = Helper.GetValue(base._message, "Channel: ");
            if (string.IsNullOrEmpty(ChannelID))
                return;

            DestinationID = Helper.GetValue(base._message, "Destination: ");
            if (string.IsNullOrEmpty(DestinationID))
            {
                DestinationID = Helper.GetValue(base._message, "DestChannel: ");
            }

            CallerIDNum = Helper.GetValue(base._message, "CallerIDNum: ");
            CallerIDName = Helper.GetValue(base._message, "CallerIDName: ");
            ConnectedLineNum = Helper.GetValue(base._message, "ConnectedLineNum: ");

            if (string.IsNullOrEmpty(ConnectedLineNum) || ConnectedLineNum == "<unknown>")
                ConnectedLineNum = Helper.GetValue(base._message, "DestCallerIDNum: ");
            if (ConnectedLineNum == "<unknown>")
            {
                ConnectedLineNum = null;
            }

            ConnectedLineName = Helper.GetValue(base._message, "ConnectedLineName: ");
            if (string.IsNullOrEmpty(ConnectedLineName) || ConnectedLineName == "<unknown>")
                ConnectedLineName = Helper.GetValue(base._message, "DestCallerIDName: ");
            if (ConnectedLineName == "<unknown>")
            {
                ConnectedLineName = null;
            }

            Uniqueid = Helper.GetValue(base._message, "Uniqueid: ");
            if (string.IsNullOrEmpty(Uniqueid))
                Uniqueid = Helper.GetValue(base._message, "DestLinkedid: ");
            if (string.IsNullOrEmpty(Uniqueid))
                Uniqueid = Helper.GetValue(base._message, "UniqueID: ");

            Uniqueid2 = Helper.GetValue(base._message, "DestUniqueid: ");
            if (string.IsNullOrEmpty(Uniqueid2))
                Uniqueid2 = Helper.GetValue(base._message, "DestUniqueID: ");

            Dialstring = Helper.GetValue(base._message, "Dialstring: ");
            if (string.IsNullOrEmpty(Dialstring))
                Dialstring = Helper.GetValue(base._message, "8: ");
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
        public DialBegin11(string _message) : base(_message)
        {
            ChannelID = Helper.GetValue(_message, "Channel: ");
            if (string.IsNullOrEmpty(ChannelID))
                return;

            DestinationID = Helper.GetValue(_message, "Destination: ");
            if (string.IsNullOrEmpty(DestinationID))
            {
                DestinationID = Helper.GetValue(_message, "DestChannel: ");
            }

            CallerIDNum = Helper.GetValue(_message, "CallerIDNum: ");
            CallerIDName = Helper.GetValue(_message, "CallerIDName: ");
            ConnectedLineNum = Helper.GetValue(_message, "ConnectedLineNum: ");

            if (string.IsNullOrEmpty(ConnectedLineNum) || ConnectedLineNum == "<unknown>")
                ConnectedLineNum = Helper.GetValue(_message, "DestCallerIDNum: ");
            if (ConnectedLineNum == "<unknown>")
            {
                ConnectedLineNum = null;
            }

            ConnectedLineName = Helper.GetValue(_message, "ConnectedLineName: ");
            if (string.IsNullOrEmpty(ConnectedLineName) || ConnectedLineName == "<unknown>")
                ConnectedLineName = Helper.GetValue(_message, "DestCallerIDName: ");
            if (ConnectedLineName == "<unknown>")
            {
                ConnectedLineName = null;
            }

            Uniqueid = Helper.GetValue(_message, "Uniqueid: ");
            if (string.IsNullOrEmpty(Uniqueid))
                Uniqueid = Helper.GetValue(_message, "DestLinkedid: ");
            if (string.IsNullOrEmpty(Uniqueid))
                Uniqueid = Helper.GetValue(_message, "UniqueID: ");

            Uniqueid2 = Helper.GetValue(_message, "DestUniqueid: ");
            if (string.IsNullOrEmpty(Uniqueid2))
                Uniqueid2 = Helper.GetValue(_message, "DestUniqueID: ");

            Dialstring = Helper.GetValue(_message, "Dialstring: ");
            if (string.IsNullOrEmpty(Dialstring))
                Dialstring = Helper.GetValue(_message, "8: ");
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
