using Proxy.Helpers;
using Proxy.ServerEntities;

namespace Proxy.Messages
{
    class ResendAction : AsteriskAction
    {
        private string action;
        public ResendAction(string message)
        {
            this._message = message;
            action = Helper.GetValue(message, "Action: ");
            ActionID = Helper.GetValue(message, "ActionID: ");
        }
        public override string Action
        {
            get
            {
                return action;
            }
        }

        public override string ToString()
        {
            return _message + Helper.LINE_SEPARATOR + Helper.LINE_SEPARATOR;
        }
    }
}
