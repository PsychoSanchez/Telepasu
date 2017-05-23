using System;

namespace Proxy.ServerEntities.Messages
{
    class CoreShowChannelsEvent : AsteriskMessage
    {
        public CoreShowChannelsEvent(string message) : base(message)
        {

        }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
