namespace Proxy.ServerEntities
{
    public abstract class ServerMessage
    {
        public MessageType type = MessageType.InnerMessage;
        public abstract string Tag { get; set; }
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
