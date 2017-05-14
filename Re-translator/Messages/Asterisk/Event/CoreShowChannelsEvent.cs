using System;

namespace Proxy.ServerEntities.Messages
{
    class CoreShowChannelsEvent : AsteriskMessage
    {
        public CoreShowChannelsEvent(string _message) : base(_message)
        {
        }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
