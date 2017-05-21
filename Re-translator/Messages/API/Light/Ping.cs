using System;

namespace Proxy.Messages.API.Light
{
    class Ping : JsonMessage
    {
        public long Timestamp;

        public Ping()
        {
            Action = "Ping";
            Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        }
    }
}
