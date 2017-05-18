using System.Text;
using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    public abstract class AsteriskMessage : ServerMessage
    {
        protected AsteriskMessageType asterType = AsteriskMessageType.Default;
        public AsteriskMessage(string _message) : base()
        {
            this._message = new StringBuilder(_message);
            EventName = Helper.GetValue(_message, "Event: ");
            if (EventName != null && EventName == "")
            {
                EventName = _message.Contains("Ping: Pong") ? "Ping" : "Response";
            }
            type = MessageType.AsteriskMessage;
        }
        public virtual string EventName { get; set; }

        public abstract string ToApi();
    }
}
