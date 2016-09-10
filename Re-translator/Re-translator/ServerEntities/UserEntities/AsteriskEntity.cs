using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proxy.ServerEntities.UserEntities
{
    public class AsteriskEntity : ServerEntity
    {
        public AsteriskEntity(Socket _client) : base(_client)
        {
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            /// Parse
            /// get list of messages
            //foreach(var message in messageList)
            //{
            //    Server.Mail.SendMessage(message);
            //}
        }

        protected override void WorkCycle()
        {
        }
    }
}
