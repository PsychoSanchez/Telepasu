using System;
using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    class DialBeginEvent : AsteriskMessage
    {
        public DialBeginEvent(string _message) : base(_message)
        {
            ChannelID = Helper.GetParameterValue(base._message, "Channel: ");
            if (string.IsNullOrEmpty(ChannelID))
                return;

            DestinationID = Helper.GetParameterValue(base._message, "Destination: ");
            if (string.IsNullOrEmpty(DestinationID))
            {
                DestinationID = Helper.GetParameterValue(base._message, "DestChannel: ");
            }

            CallerIDNum = Helper.GetParameterValue(base._message, "CallerIDNum: ");
            CallerIDName = Helper.GetParameterValue(base._message, "CallerIDName: ");
            ConnectedLineNum = Helper.GetParameterValue(base._message, "ConnectedLineNum: ");

            if (string.IsNullOrEmpty(ConnectedLineNum) || ConnectedLineNum == "<unknown>")
                ConnectedLineNum = Helper.GetParameterValue(base._message, "DestCallerIDNum: ");
            if (ConnectedLineNum == "<unknown>")
            {
                ConnectedLineNum = null;
            }

            ConnectedLineName = Helper.GetParameterValue(base._message, "ConnectedLineName: ");
            if (string.IsNullOrEmpty(ConnectedLineName) || ConnectedLineName == "<unknown>")
                ConnectedLineName = Helper.GetParameterValue(base._message, "DestCallerIDName: ");
            if (ConnectedLineName == "<unknown>")
            {
                ConnectedLineName = null;
            }

            Uniqueid = Helper.GetParameterValue(base._message, "Uniqueid: ");
            if (string.IsNullOrEmpty(Uniqueid))
                Uniqueid = Helper.GetParameterValue(base._message, "DestLinkedid: ");
            if (string.IsNullOrEmpty(Uniqueid))
                Uniqueid = Helper.GetParameterValue(base._message, "UniqueID: ");

            Uniqueid2 = Helper.GetParameterValue(base._message, "DestUniqueid: ");
            if (string.IsNullOrEmpty(Uniqueid2))
                Uniqueid2 = Helper.GetParameterValue(base._message, "DestUniqueID: ");

            Dialstring = Helper.GetParameterValue(base._message, "Dialstring: ");
            if (string.IsNullOrEmpty(Dialstring))
                Dialstring = Helper.GetParameterValue(base._message, "8: ");
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
            ChannelID = Helper.GetParameterValue(_message, "Channel: ");
            if (string.IsNullOrEmpty(ChannelID))
                return;

            DestinationID = Helper.GetParameterValue(_message, "Destination: ");
            if (string.IsNullOrEmpty(DestinationID))
            {
                DestinationID = Helper.GetParameterValue(_message, "DestChannel: ");
            }

            CallerIDNum = Helper.GetParameterValue(_message, "CallerIDNum: ");
            CallerIDName = Helper.GetParameterValue(_message, "CallerIDName: ");
            ConnectedLineNum = Helper.GetParameterValue(_message, "ConnectedLineNum: ");

            if (string.IsNullOrEmpty(ConnectedLineNum) || ConnectedLineNum == "<unknown>")
                ConnectedLineNum = Helper.GetParameterValue(_message, "DestCallerIDNum: ");
            if (ConnectedLineNum == "<unknown>")
            {
                ConnectedLineNum = null;
            }

            ConnectedLineName = Helper.GetParameterValue(_message, "ConnectedLineName: ");
            if (string.IsNullOrEmpty(ConnectedLineName) || ConnectedLineName == "<unknown>")
                ConnectedLineName = Helper.GetParameterValue(_message, "DestCallerIDName: ");
            if (ConnectedLineName == "<unknown>")
            {
                ConnectedLineName = null;
            }

            Uniqueid = Helper.GetParameterValue(_message, "Uniqueid: ");
            if (string.IsNullOrEmpty(Uniqueid))
                Uniqueid = Helper.GetParameterValue(_message, "DestLinkedid: ");
            if (string.IsNullOrEmpty(Uniqueid))
                Uniqueid = Helper.GetParameterValue(_message, "UniqueID: ");

            Uniqueid2 = Helper.GetParameterValue(_message, "DestUniqueid: ");
            if (string.IsNullOrEmpty(Uniqueid2))
                Uniqueid2 = Helper.GetParameterValue(_message, "DestUniqueID: ");

            Dialstring = Helper.GetParameterValue(_message, "Dialstring: ");
            if (string.IsNullOrEmpty(Dialstring))
                Dialstring = Helper.GetParameterValue(_message, "8: ");
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
