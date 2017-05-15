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
            if (EventName != null && EventName == "")
            {
                EventName = _message.Contains("Ping: Pong") ? "Ping" : "Response";
            }
            Tag = "AsteriskNativeMessage";
            type = MessageType.AsteriskMessage;
        }

        public sealed override string Tag { get; set; }
        public virtual string EventName { get; set; }

        public abstract override string ToApi();

        public override string ToString()
        {
            return _message;
        }
    }
}
