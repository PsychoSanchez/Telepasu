using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;
using Proxy.Messages.API.Admin;

namespace Proxy.ServerEntities.Users
{
    class AdminUser : UserManager
    {
        public AdminUser(Socket client) : base(client)
        {
            Role = UserRole.Admin;
            PersonalMail.SendMessage(JsonConvert.SerializeObject(new AuthResponse(true)));
        }

        protected override void Disconnected(object sender, MessageArgs e)
        {
            Cts.Cancel();
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            telepasu.log("Message recieved");
        }

        protected override void WorkCycle()
        {
            while (true)
            {
                if (Cts.Token.IsCancellationRequested)
                {
                    telepasu.log("User disconnect");
                    return;
                }
                List<ServerMessage> messages = Server.Mail.GrabMessages(this);
                foreach(var message in messages)
                {
                    PersonalMail.SendMessage(message.ToString());
                }
                Cts.Token.WaitHandle.WaitOne(300);
            }
        }
    }
}
