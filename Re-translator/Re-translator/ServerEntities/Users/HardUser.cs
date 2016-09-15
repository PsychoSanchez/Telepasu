using Proxy.Messages.API;
using Proxy.ServerEntities.Asterisk;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Proxy.ServerEntities.Users
{

    class HardUser : UserManager
    {
        MessagesParser parser = new MessagesParser();
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
                    var pingAction = new PingEvent();
                    if (message.ActionID != null)
                    {
                        pingAction.ActionID = message.ActionID;
                    }
                    personal_mail.SendMessage(pingAction.ToString());
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
                    Console.WriteLine("User disconnect");
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
