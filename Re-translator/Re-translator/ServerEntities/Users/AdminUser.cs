using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Proxy.ServerEntities.Users
{
    class AdminUser : UserManager
    {
        public AdminUser(Socket _client) : base(_client)
        {
            role = UserRole.Admin;
        }

        protected override void Disconnected(object sender, MessageArgs e)
        {
            cts.Cancel();
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            telepasu.log("Message recieved");
        }

        protected override void WorkCycle()
        {
            while (true)
            {
                if (cts.Token.IsCancellationRequested)
                {
                    telepasu.log("User disconnect");
                    return;
                }
                List<ServerMessage> messages = Server.Mail.GrabMessages(this);
                foreach(var message in messages)
                {
                    personal_mail.SendMessage(message.ToString());
                }
                cts.Token.WaitHandle.WaitOne(300);
            }
        }
    }
}
