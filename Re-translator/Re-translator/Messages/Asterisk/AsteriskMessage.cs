using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    public abstract class AsteriskMessage : ServerMessage
    {
        protected AsteriskMessageType asterType = AsteriskMessageType.Default;
        public AsteriskMessage(string _message) : base()
        {
            this._message = _message;
            EventName = Helper.GetValue(_message, "Event: ");
            if (EventName == "")
            {
                if (_message.Contains("Ping: Pong"))
                {
                    EventName = "Ping";
                }
                else
                {
                    EventName = "Response";
                }
            }
            type = MessageType.AsteriskMessage;
        }
        public virtual string EventName { get; set; }

        public abstract override string ToApi();

        public override string ToString()
        {
            return _message;
        }
    }
}
