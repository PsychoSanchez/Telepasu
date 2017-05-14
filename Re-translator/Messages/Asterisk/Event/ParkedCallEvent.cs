using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities.Messages
{
    class ParkedCallEvent : AsteriskMessage
    {
        public ParkedCallEvent(string _message) : base(_message)
        {
            Channel = Helper.GetValue(_message, "Channel: ");
            CallerIDNum = Helper.GetValue(_message, "CallerIDNum: ");
            CallerIDName = Helper.GetValue(_message, "CallerIDName: ");
            Exten = Helper.GetValue(_message, "Exten: ");
            Timeout = Helper.GetValue(_message, "Timeout: ");
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
