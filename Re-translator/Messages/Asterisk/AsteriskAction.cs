using Proxy.Helpers;

namespace Proxy.ServerEntities
{
    public abstract class AsteriskAction : ServerMessage
    {
        public string ActionId { get; protected set; }

        protected AsteriskAction()
        {
            Tag = "AsteriskNativeAction";
            type = MessageType.AsteriskAction;
        }
        public abstract string Action { get; }

        public override string ToString()
        {
            return Helper.ToString(this);
        }
    }
}
