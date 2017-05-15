using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;
using Proxy.Helpers;
using Proxy.Messages.API.Admin;

namespace Proxy.ServerEntities.Users
{
    internal class AdminUser : UserManager
    {
        public AdminUser(SocketMail mail) : base(mail)
        {
            Role = UserRole.Admin;
            PersonalMail.IsApi = true;
            PersonalMail.SendApiMessage(JsonConvert.SerializeObject(new AuthResponse(true)));
        }

        protected override void Disconnected(object sender, MessageArgs e)
        {
            Cts.Cancel();
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            telepasu.log(e.Message);
            var action = Helper.GetJsonValue(e.Message, "action");
            telepasu.log(action);
            switch (action)
            {
                case "Add Module":
                    var commmand = JsonConvert.DeserializeObject<AddModule>(e.Message);
                    Console.WriteLine(commmand);
                    break;
                default:
                    break;
            }
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
                List<ServerMessage> messages = Server.MailPost.GrabMessages(this);
                foreach (var message in messages)
                {
                    PersonalMail.SendMessage(message.ToString());
                }
                Cts.Token.WaitHandle.WaitOne(300);
            }
        }
    }
}
