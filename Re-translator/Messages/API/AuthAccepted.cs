using System.Text;
using Proxy.ServerEntities;
using Proxy.Helpers;

namespace Proxy.Messages.API
{
    class AuthAccepted : ServerMessage
    {
        public AuthAccepted(string actionId)
        {
            this._message = new StringBuilder("Response: Success" + Helper.LINE_SEPARATOR);
            if (actionId != null)
            {
                this._message.Append("ActionID: " + actionId + Helper.LINE_SEPARATOR);
            }
            this._message.Append("Message: Authentication accepted" + Helper.LINE_SEPARATOR + Helper.LINE_SEPARATOR);
        }

        public string Tag = "Asterisk Native";
    }
}
