using Proxy.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.ServerEntities.Messages
{
    class PingEvent : AsteriskMessage
    {
        string timestamp;
        public PingEvent(string _message) : base(_message)
        {
            timestamp = Helper.GetParameterValue(_message, "Timestamp: ");
        }

        public string Timestamp
        {
            get
            {
                return timestamp;
            }

            set
            {
                timestamp = value;
            }
        }

        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
