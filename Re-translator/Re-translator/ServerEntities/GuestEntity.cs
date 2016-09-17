using Proxy.Helpers;
using Proxy.Messages.API;
using System;
using System.Net.Sockets;

namespace Proxy.ServerEntities.Users
{
    class GuestEntity : UserManager
    {
        Socket client;
        ConnectionTimer timer;
        UserManager user;
        string challenge;
        public GuestEntity(Socket _client, int timeout) : base()
        {
            this.client = _client;
            timer = new ConnectionTimer(timeout);
        }

        public UserManager StartAutorization()
        {
            Listen(client);
            timer.Wait();
            return user;
        }
        protected override void Disconnected(object sender, MessageArgs e)
        {
            telepasu.log(UserName + " disconnected");
            return;
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            var actions = parser.ToActionList(e.Message);
            foreach (var message in actions)
            {
                if (message.Action == "Challenge")
                {
                    challenge = Encryptor.GenerateChallenge();
                    var aster_action = new Challenge(e.Message, challenge);
                    personal_mail.SendMessage(aster_action.ToString());
                    timer.Reset();
                }
                else if (message.Action == "Login")
                {
                    var username = Helper.GetValue(e.Message, "UserName: ");
                    if (username == "")
                    {
                        username = Helper.GetValue(e.Message, "Username: ");
                    }
                    var pwd = Helper.GetValue(e.Message, "Key: ");
                    var actionID = Helper.GetValue(e.Message, "ActionID: ");
                    if (challenge != null)
                    {
                        authentificated = Server.Mail.DB.Authentificate(username, pwd, challenge);
                    }
                    else
                    {
                        authentificated = Server.Mail.DB.Authentificate(username, pwd);
                    }
                    if (authentificated)
                    {
                        UserName = username;
                        personal_mail.StopListen();
                        user = new HardUser(client, actionID);
                    }
                    else
                    {
                        Shutdown();
                    }
                    timer.StopWait();
                }
                else if (message.Action == "Auth")
                {
                    telepasu.log("Auth not implemented yet");
                    Shutdown();
                    timer.StopWait();
                }
                else if (message.Action == "Ping")
                {
                    var pingAction = new PingEvent();
                    if (message.ActionID != "")
                    {
                        pingAction.ActionID = message.ActionID;
                    }
                    personal_mail.SendMessage(pingAction.ToString());
                    return;
                }
                else
                {
                    Shutdown();
                    timer.StopWait();
                }
            }
            return;
        }

        protected override void WorkCycle()
        {
            return;
        }
    }
}
