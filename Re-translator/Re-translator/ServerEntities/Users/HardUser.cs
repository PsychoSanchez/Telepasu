using Proxy.Helpers;
using Proxy.Messages.API;
using Proxy.ServerEntities.Asterisk;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Proxy.ServerEntities.Users
{

    class HardUser : UserManager
    {
        public HardUser(Socket _client, string actionID) : base(_client)
        {
            role = UserRole.HardUser;
            AuthAccepted aciton = new AuthAccepted(actionID);
            personal_mail.SendMessage(aciton.ToString());
        }

        protected override void Disconnected(object sender, MessageArgs e)
        {
            cts.Cancel();
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            var actions = parser.ToActionList(e.Message);
            foreach (var message in actions)
            {
                if (message.Action == "Ping")
                {
                    string action = "";
                    action = "Response: Success" + Helper.LINE_SEPARATOR;
                    //PingEvent pingAction = new PingEvent();
                    if (message.ActionID != null)
                    {
                        action += "ActionID: " + message.ActionID + Helper.LINE_SEPARATOR;
                    }
                    action += "Ping: Pong" + Helper.LINE_SEPARATOR;
                    string unixTimestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString().Replace(',', '.');
                    action += "Timestamp: " + unixTimestamp + Helper.LINE_SEPARATOR;
                    action += Helper.LINE_SEPARATOR;
                    personal_mail.SendMessage(action);
                }
                else if (message.Action == "Logoff")
                {
                    Shutdown();
                }
                else
                {
                    Server.Mail.PostMessage(message);
                }
            }
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
                foreach (var message in messages)
                {
                    personal_mail.SendMessage(message.ToString());
                }
                cts.Token.WaitHandle.WaitOne(300);
            }
        }
    }
}
