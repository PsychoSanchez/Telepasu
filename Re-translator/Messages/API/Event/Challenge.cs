﻿using Proxy.Helpers;
using Proxy.ServerEntities;
using System.Text;

namespace Proxy.Messages.API
{
    class Challenge : ServerMessage
    {        
        public Challenge(string message, string challenge)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Asterisk Call Manager/" + Server.Mail.AsteriskVersion + Helper.LINE_SEPARATOR);
            sb.Append("Response: Success" + Helper.LINE_SEPARATOR);
            var actionID = Helper.GetValue(message, "ActionID: ");
            if (actionID != "")
            {
                sb.Append("ActionID: " + actionID + Helper.LINE_SEPARATOR);
            }
            sb.Append("Challenge: " + challenge + Helper.LINE_SEPARATOR);
            sb.Append(Helper.LINE_SEPARATOR);
            this._message = sb.ToString();
        }
        public override string ToString()
        {
            return _message;
        }
    }
}