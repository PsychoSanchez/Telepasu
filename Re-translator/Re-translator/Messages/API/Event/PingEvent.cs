using System;
using Proxy.ServerEntities;
using Proxy.Helpers;

namespace Proxy.Messages.API
{
    class PingEvent : ServerMessage
    {
        public string ActionID { get; set; }
        public PingEvent()
        {
            _message = "Response: Success" + Helper.LINE_SEPARATOR;
        }
        public override string ToString()
        {
            string unixTimestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString().Replace(',', '.');
            if (ActionID != null)
            {
                _message += "ActionID: " + ActionID + Helper.LINE_SEPARATOR;
            }
            _message += "Ping: Pong" + Helper.LINE_SEPARATOR;
            _message += "Timestamp: " + unixTimestamp + Helper.LINE_SEPARATOR;
            _message += Helper.LINE_SEPARATOR;
            return _message;
        }
    }
}
