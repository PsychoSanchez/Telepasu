using Proxy.Helpers;
using System;

namespace Proxy.ServerEntities
{
    public abstract class AsteriskAction : ServerMessage
    {
        protected string actionID;
        public virtual string ActionID
        {
            get
            {
                return actionID;
            }

            set
            {
                actionID = value;
            }
        }
        public AsteriskAction()
        {
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
