using System;

namespace Proxy.ServerEntities.Messages
{
    class ParkedCallsCompleteEvent : AsteriskMessage
    {
        public ParkedCallsCompleteEvent(string _message) : base(_message)
        {
        }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
