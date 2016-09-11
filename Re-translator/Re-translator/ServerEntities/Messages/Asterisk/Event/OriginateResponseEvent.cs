using System;
using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    class OriginateResponseEvent : AsteriskMessage
    {
        public OriginateResponseEvent(string _message) : base(_message)
        {
            if (Helper.GetParameterValue(_message, "Response: ").Equals("Failure"))
                Response = false;
            else
                Response = true;
            Channel = Helper.GetParameterValue(_message, "Channel: ");
            Context = Helper.GetParameterValue(_message, "Context: ");
            Exten = Helper.GetParameterValue(_message, "Exten: ");
            Reason = Helper.GetParameterValue(_message, "Reason: ");
            Uniqueid = Helper.GetParameterValue(_message, "Uniqueid: ");
            CallerIDNum = Helper.GetParameterValue(_message, "CallerIDNum: ");
            CallerIDName = Helper.GetParameterValue(_message, "CallerIDName: ");
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
