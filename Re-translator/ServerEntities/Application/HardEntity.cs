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
                        var action = new StringBuilder("");
                        action.Append("Response: Success" + Helper.LINE_SEPARATOR);
                        PingEvent pingAction = new PingEvent();
                        if (message.ActionId != null)
                        {
                            action.Append("ActionID: " + message.ActionId + Helper.LINE_SEPARATOR);
                        }
                        action.Append("Ping: Pong" + Helper.LINE_SEPARATOR);
                        var unixTimestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString(CultureInfo.InvariantCulture).Replace(',', '.');
                        action.Append("Timestamp: " + unixTimestamp + Helper.LINE_SEPARATOR);
                        action.Append(Helper.LINE_SEPARATOR);
                        PersonalMail.SendMessage(action.ToString());
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
