using System.Text;

namespace Proxy.ServerEntities
{
    public abstract class ServerMessage
    {
        public MessageType type = MessageType.InnerMessage;
        public string Tag;
        protected StringBuilder _message = new StringBuilder("");
        public ServerMessage()
        {
        }

        public new virtual string ToString()
        {
            return _message.ToString();
        }
        public string ToApi()
        {
            return _message.ToString();
        }
    }
}
