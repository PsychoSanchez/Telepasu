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
        public string ActionId { get; set; }

        string message;

        public PingEvent()
        {
            var unix = DateTimeOffset.Now.ToUnixTimeSeconds();
            _message.Append(Response);
            if (ActionId != null)
            {
                _message.Append(Action + ActionId + Helper.LINE_SEPARATOR);
            }
            _message.Append(Ping);
            _message.Append(Timestamp + unix + Helper.LINE_SEPARATOR);
            _message.Append(Helper.LINE_SEPARATOR);
        }
    }
}
