using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;
using Proxy.Engine;
using Proxy.Helpers;
using Proxy.Messages.API.Admin;

namespace Proxy.ServerEntities.Application
{
    internal class AdminEntity : EntityManager
    {
        public AdminEntity(SocketMail mail) : base(mail)
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
                    var command = JsonConvert.DeserializeObject<AddModuleCommand>(e.Message);
                    ProxyEngine.MailPost.PostMessage(command);
                    break;
                default:
                    break;
            }
        }

    }
}
