using System.Text;
using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    public abstract class AsteriskMessage : ServerMessage
    {
        protected AsteriskMessageType asterType = AsteriskMessageType.Default;
        public AsteriskMessage(string message) : base()
        {
            this.Message = new StringBuilder(message);
            EventName = Helper.GetValue(message, "Event: ");
            if (EventName != null && EventName == "")
            {
                EventName = message.Contains("Ping: Pong") ? "Ping" : "Response";
            }
            type = MessageType.AsteriskMessage;
        }
        public string EventName { get; set; }

        public abstract string ToApi();
    }
}
