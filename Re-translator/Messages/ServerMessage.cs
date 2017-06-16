using System.Text;

namespace Proxy.ServerEntities
{
    public class ServerMessage
    {
        public MessageType type = MessageType.InnerMessage;
        public string Tag;
        protected StringBuilder Message = new StringBuilder("");

        public ServerMessage()
        {
            
        }

        public ServerMessage(string message)
        {
            Message.Append(message);
        }

        public new virtual string ToString()
        {
            return Message.ToString();
        }
        public string ToApi()
        {
            return Message.ToString();
        }
    }
}
