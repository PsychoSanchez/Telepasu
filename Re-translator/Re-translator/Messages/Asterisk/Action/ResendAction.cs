using Proxy.Helpers;
using Proxy.ServerEntities;

namespace Proxy.Messages
{
    class ResendAction : AsteriskAction
    {
        public ResendAction(string message)
        {
            this._message = message;
        }
        public override string Action
        {
            get
            {
                return "";
            }
        }
        public override string ToString()
        {
            return _message + Helper.LINE_SEPARATOR + Helper.LINE_SEPARATOR;
        }
    }
}
