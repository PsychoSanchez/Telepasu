using System.Text;
using Proxy.Helpers;
using Proxy.ServerEntities;

namespace Proxy.Messages
{
    class ResendAction : AsteriskAction
    {
        public ResendAction(string message)
        {
            this._message = new StringBuilder(message);
            Action = Helper.GetValue(message, "Action: ");
            ActionId = Helper.GetValue(message, "ActionID: ");
        }
        public override string Action { get; }

        public override string ToString()
        {
            return _message.Append(Helper.LINE_SEPARATOR + Helper.LINE_SEPARATOR).ToString();
        }
    }
}
