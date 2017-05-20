using Proxy.Helpers;
using Proxy.Messages.API;
using Proxy.ServerEntities.Asterisk;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using Proxy.Engine;

namespace Proxy.ServerEntities.Application
{

    class HardEntity : EntityManager
    {
        public HardEntity(SocketMail mail, string actionId) : base(mail)
        {
            Role = UserRole.HardUser;
            AuthAccepted aciton = new AuthAccepted(actionId);
            PersonalMail.SendMessage(aciton.ToString());
        }

        protected override void Disconnected(object sender, MessageArgs e)
        {
            Cts.Cancel();
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            var actions = Parser.ToActionList(e.Message);
            foreach (var message in actions)
            {
                switch (message.Action)
                {
                    case "Ping":
                        PingEvent pingAction = new PingEvent(message.ActionId);
                        PersonalMail.SendMessage(pingAction.ToString());
                        break;
                    case "Logoff":
                        Shutdown();
                        break;
                    default:
                        message.Tag = NativeModulesTags.Asterisk + NativeModulesTags.Incoming;
                        ProxyEngine.MailPost.PostMessage(message);
                        break;
                }
            }
        }
    }
}
