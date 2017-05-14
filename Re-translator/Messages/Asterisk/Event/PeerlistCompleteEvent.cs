using System.Threading;
using System;
using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    class PeerlistCompleteEvent : AsteriskMessage
    {
        public static readonly AutoResetEvent PeerlistComplete = new AutoResetEvent(false);

        public PeerlistCompleteEvent(string _message) : base(_message)
        {
            ListItems = Helper.GetValue(_message, "ListItems: ");
            ActionID = Helper.GetValue(_message, "ActionID: ");
        }

        public string ActionID { get; private set; }
        public string ListItems { get; private set; }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
