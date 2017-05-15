using Proxy.Helpers;

namespace Proxy.ServerEntities
{
    public abstract class AsteriskAction : ServerMessage
    {
        private string _actionId;
        public sealed override string Tag { get; set; }

        public virtual string ActionId
        {
            get
            {
                return _actionId;
            }

            set
            {
                _actionId = value;
            }
        }
        public AsteriskAction()
        {
            Tag = "AsteriskNativeAction";
            type = MessageType.AsteriskAction;
        }
        public abstract string Action { get; }

        private string action;

        public override string ToString()
        {
            return Helper.ToString(this);
        }
    }
}
