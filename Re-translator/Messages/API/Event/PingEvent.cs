using System;
using Proxy.ServerEntities;
using Proxy.Helpers;

namespace Proxy.Messages.API
{
    class PingEvent : ServerMessage
    {
        private const string Response = "Response: Success\r\n";
        private const string Ping = "Ping: Pong\r\n";
        private const string Timestamp = "Timestamp: ";
        private const string Action = "ActionID: ";

        public PingEvent(string actionId)
        {
            var unix = DateTimeOffset.Now.ToUnixTimeSeconds();
            Message.Append(Response);
            if (!string.IsNullOrEmpty(actionId))
            {
                Message.Append(Action + actionId + Helper.LINE_SEPARATOR);
            }
            Message.Append(Ping);
            Message.Append(Timestamp + unix + Helper.LINE_SEPARATOR);
            Message.Append(Helper.LINE_SEPARATOR);
        }
    }
}
