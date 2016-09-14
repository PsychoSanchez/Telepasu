using Proxy.Helpers;
using System;
using System.Threading;

namespace Proxy.ServerEntities.Messages
{
    class OriginateEvent : AsteriskMessage
    {
        public static readonly AutoResetEvent OriginateComplete = new AutoResetEvent(false);

        public OriginateEvent(string _message) : base(_message)
        {
            ActionID = Helper.GetValue(_message, "ActionID: ");
            if (string.IsNullOrEmpty(ActionID))
            {
                IsCallSuccess = false;
            }
            else
            {
                IsCallSuccess = true;
            }
        }

        public string ActionID { get; private set; }
        public bool IsCallSuccess { get; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
