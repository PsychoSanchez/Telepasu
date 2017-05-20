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
            Tag = NativeModulesTags.Asterisk;

            Message.Append("Asterisk Call Manager/" + ProxyEngine.MailPost.AsteriskVersion + Helper.LINE_SEPARATOR);
            Message.Append("Response: Success" + Helper.LINE_SEPARATOR);
            var actionId = Helper.GetValue(message, "ActionID: ");
            if (actionId != "")
            {
                Message.Append("ActionID: " + actionId + Helper.LINE_SEPARATOR);
            }
            Message.Append("Challenge: " + challenge + Helper.LINE_SEPARATOR);
            Message.Append(Helper.LINE_SEPARATOR);
        }
    }
}
