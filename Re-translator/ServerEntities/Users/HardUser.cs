using Proxy.Helpers;
using Proxy.Messages.API;
using Proxy.ServerEntities.Asterisk;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;

namespace Proxy.ServerEntities.Users
{

    class HardUser : UserManager
    {
        public HardUser(SocketMail mail, string actionId) : base(mail)
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
                        var action = "";
                        action = "Response: Success" + Helper.LINE_SEPARATOR;
                        //PingEvent pingAction = new PingEvent();
                        if (message.ActionID != null)
                        {
                            action += "ActionID: " + message.ActionID + Helper.LINE_SEPARATOR;
                        }
                        action += "Ping: Pong" + Helper.LINE_SEPARATOR;
                        var unixTimestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString(CultureInfo.InvariantCulture).Replace(',', '.');
                        action += "Timestamp: " + unixTimestamp + Helper.LINE_SEPARATOR;
                        action += Helper.LINE_SEPARATOR;
                        PersonalMail.SendMessage(action);
                        break;
                    case "Logoff":
                        Shutdown();
                        break;
                    default:
                        Server.MailPost.PostMessage(message);
                        break;
                }
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
