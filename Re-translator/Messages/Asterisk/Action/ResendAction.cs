using System.Text;
using Proxy.Helpers;
using Proxy.ServerEntities;

namespace Proxy.Messages
{
    class ResendAction : AsteriskAction
    {
        public ResendAction(string message)
        {
            this.Message = new StringBuilder(message);
            Action = Helper.GetValue(message, "Action: ");
            ActionId = Helper.GetValue(message, "ActionID: ");
        }
        public override string Action { get; }

        public override string ToString()
        {
            return Message.Append(Helper.LINE_SEPARATOR + Helper.LINE_SEPARATOR).ToString();
        }
    }
}
