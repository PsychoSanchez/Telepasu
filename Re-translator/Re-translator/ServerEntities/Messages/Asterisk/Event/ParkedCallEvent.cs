using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities.Messages
{
    class ParkedCallEvent : AsteriskMessage
    {
        public ParkedCallEvent(string _message) : base(_message)
        {
            Channel = Helper.GetParameterValue(_message, "Channel: ");
            CallerIDNum = Helper.GetParameterValue(_message, "CallerIDNum: ");
            CallerIDName = Helper.GetParameterValue(_message, "CallerIDName: ");
            Exten = Helper.GetParameterValue(_message, "Exten: ");
            Timeout = Helper.GetParameterValue(_message, "Timeout: ");
        }

        public string CallerIDName { get; private set; }
        public string CallerIDNum { get; private set; }
        public string Channel { get; private set; }
        public string Exten { get; private set; }
        public string Timeout { get; private set; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
