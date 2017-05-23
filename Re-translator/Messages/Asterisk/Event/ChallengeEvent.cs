using Proxy.Helpers;
using System;
using System.Threading;

namespace Proxy.ServerEntities.Messages
{
    class ChallengeEvent : AsteriskMessage
    {
        public ChallengeEvent(string message) : base(message)
        {
            Version = Helper.GetValue(message, "Asterisk Call Manager/");
            Challenge = Helper.GetValue(message, "Challenge: ");
            ActionID = Helper.GetValue(message, "ActionID: ");
        }
        public string ActionID { get; private set; }
        public string Challenge { get; private set; }
        public string Version { get; private set; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
