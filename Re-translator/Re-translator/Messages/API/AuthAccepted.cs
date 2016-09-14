using Proxy.ServerEntities;
using Proxy.Helpers;

namespace Proxy.Messages.API
{
    class AuthAccepted : ServerMessage
    {
        public AuthAccepted(string actionId)
        {
            this._message = "Response: Success" + Helper.LINE_SEPARATOR;
            if (actionId != null)
            {
                this._message += "ActionID: " + actionId + Helper.LINE_SEPARATOR;
            }
            this._message += "Message: Authentication accepted" + Helper.LINE_SEPARATOR + Helper.LINE_SEPARATOR;
        }
        public override string ToString()
        {
            return this._message;
        }
    }
}
