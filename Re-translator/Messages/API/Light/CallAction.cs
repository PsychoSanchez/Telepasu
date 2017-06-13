using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Messages.API.Light
{
    class CallAction
    {
        public string Number;
        public string Exten;
        public string Action { get; set; }

        public string Guid { get; set; }

        public CallAction(string guid, string number, string exten)
        {
            Guid = guid;
            Action = "AsteriskCall";
            Number = number;
            Exten = exten;
        }
    }
}
