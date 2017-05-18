using Proxy.Helpers;
using Proxy.ServerEntities;
using System.Text;
using Proxy.Engine;

namespace Proxy.Messages.API
{
    class Challenge : ServerMessage
    {        
        public Challenge(string message, string challenge)
        {
            _message.Append("Asterisk Call Manager/" + ProxyEngine.MailPost.AsteriskVersion + Helper.LINE_SEPARATOR);
            _message.Append("Response: Success" + Helper.LINE_SEPARATOR);
            var actionId = Helper.GetValue(message, "ActionID: ");
            if (actionId != "")
            {
                _message.Append("ActionID: " + actionId + Helper.LINE_SEPARATOR);
            }
            _message.Append("Challenge: " + challenge + Helper.LINE_SEPARATOR);
            _message.Append(Helper.LINE_SEPARATOR);
        }

        public string Tag = "Asterisk Native";
    }
}
