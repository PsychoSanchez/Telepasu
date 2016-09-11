namespace Proxy.ServerEntities
{
    public abstract class ServerMessage
    {
        public MessageType type = MessageType.InnerMessage;
        protected string _message = string.Empty;
        public ServerMessage()
        {
        }
        public abstract override string ToString();
        public virtual string ToApi()
        {
            return _message;
        }
    }
}
