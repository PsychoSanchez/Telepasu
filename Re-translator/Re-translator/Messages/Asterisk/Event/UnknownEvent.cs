using System;

namespace Proxy.ServerEntities.Messages
{
    class UnknownEvent : AsteriskMessage
    {
        public UnknownEvent(string _message) : base(_message)
        {
            telepasu.log(_message);
        }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
