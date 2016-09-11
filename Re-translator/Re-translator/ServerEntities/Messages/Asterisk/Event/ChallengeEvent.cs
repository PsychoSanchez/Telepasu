using Proxy.Helpers;
using System;
using System.Threading;

namespace Proxy.ServerEntities.Messages
{
    class ChallengeEvent : AsteriskMessage
    {
        public ChallengeEvent(string _message) : base(_message)
        {
            Version = Helper.GetParameterValue(_message, "Asterisk Call Manager/");
            Challenge = Helper.GetParameterValue(_message, "Challenge: ");
            ActionID = Helper.GetParameterValue(_message, "ActionID: ");
        }
        public string ActionID { get; private set; }
        public string Challenge { get; }
        public string Version { get; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
