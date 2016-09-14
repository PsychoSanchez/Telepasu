using System;
using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    class OriginateResponseEvent : AsteriskMessage
    {
        public OriginateResponseEvent(string _message) : base(_message)
        {
            if (Helper.GetValue(_message, "Response: ").Equals("Failure"))
                Response = false;
            else
                Response = true;
            Channel = Helper.GetValue(_message, "Channel: ");
            Context = Helper.GetValue(_message, "Context: ");
            Exten = Helper.GetValue(_message, "Exten: ");
            Reason = Helper.GetValue(_message, "Reason: ");
            Uniqueid = Helper.GetValue(_message, "Uniqueid: ");
            CallerIDNum = Helper.GetValue(_message, "CallerIDNum: ");
            CallerIDName = Helper.GetValue(_message, "CallerIDName: ");
        }

        public bool Response { get; private set; }
        public string Channel { get; private set; }
        public string Context { get; private set; }
        public string Exten { get; private set; }
        public string Reason { get; private set; }
        public string Uniqueid { get; private set; }
        public string CallerIDNum { get; private set; }
        public string CallerIDName { get; private set; }


        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
