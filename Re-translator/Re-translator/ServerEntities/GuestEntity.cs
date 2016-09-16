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
            timer = new ConnectionTimer(10000);
        }

        public UserManager StartAutorization()
        {
            Listen(client);
            timer.Wait();
            return user;
        }
        protected override void Disconnected(object sender, MessageArgs e)
        {
            telepasu.log("guest disconnected");
            return;
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            var action = Helper.GetValue(e.Message, "Action: ");
            if (action == "Challenge")
            {
                challenge = Encryptor.GenerateChallenge();
                var aster_action = new Challenge(e.Message, challenge);
                personal_mail.SendMessage(aster_action.ToString());
                timer.Reset();
            }
            else if (action == "Login")
            {
                var username = Helper.GetValue(e.Message, "UserName: ");
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
                    user = new HardUser(client, actionID);
                    personal_mail.StopListen();
                }
                else
                {
                    Shutdown();
                }
                timer.StopWait();
            }
            else if (action == "Auth")
            {
                Shutdown();
                timer.StopWait();
            }
            else
            {
                Shutdown();
                timer.StopWait();
            }
            return;
        }

        protected override void WorkCycle()
        {
            return;
        }
    }
}
