﻿using System.Text;
using Proxy.ServerEntities;
using Proxy.Helpers;

namespace Proxy.Messages.API
{
    class AuthAccepted : ServerMessage
    {
        public AuthAccepted(string actionId)
        {
            this.Message = new StringBuilder("Response: Success" + Helper.LINE_SEPARATOR);
            if (actionId != null)
            {
                this.Message.Append("ActionID: " + actionId + Helper.LINE_SEPARATOR);
            }
            this.Message.Append("Message: Authentication accepted" + Helper.LINE_SEPARATOR + Helper.LINE_SEPARATOR);
            Tag = "Asterisk Native";
        } 
    }
}
