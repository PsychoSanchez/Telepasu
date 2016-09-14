namespace Proxy.ServerEntities.Messages
{
    public abstract class AsteriskMessage : ServerMessage
    {
        protected AsteriskMessageType asterType = AsteriskMessageType.Default;
        public AsteriskMessage(string _message) : base()
        {
            this._message = _message;
            type = MessageType.AsteriskMessage;
        }

        public abstract override string ToApi();

        public override string ToString()
        {
            return _message;
        }
    }
}
