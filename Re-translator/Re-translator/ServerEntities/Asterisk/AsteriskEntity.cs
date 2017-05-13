using Proxy.Helpers;
using Proxy.ServerEntities.Asterisk;
using Proxy.ServerEntities.Messages;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Proxy.ServerEntities.Users
{
    public class AsteriskEntity : UserManager
    {
        private bool connected;
        public string version;
        private MessagesParser parser = new MessagesParser();
        private AutoResetEvent response = new AutoResetEvent(false);
        private List<AsteriskMessage> inner_messages = new List<AsteriskMessage>();
        public AsteriskEntity(Socket _client) : base(_client)
        {
        }
        public bool Connected
        {
            get
            {
                return connected;
            }

            set
            {
                connected = value;
            }
        }
        public bool Login(string login, string password)
        {
            inner_messages.Clear();

            ///First step - Send Challenge to get prefix for MD5 hash
            AsteriskAction action = new ChallengeAction();
            var message = action.ToString();
            PersonalMail.SendMessage(message);
            if (!response.WaitOne(5000))
            {
                return false;
            }

            ///Second step - Encrypt user pwd and send data to server
            var challengeResult = (ChallengeEvent)inner_messages[0];
            Server.Mail.AsteriskVersion = challengeResult.Version;
            string key = Encryptor.CalculateMD5Hash(challengeResult.Challenge + password);


            action = new LoginAction(login, "MD5", key, null);
            message = action.ToString();
            PersonalMail.SendMessage(message);
            if (!response.WaitOne(5000))
            {
                return false;
            }
            var loginResult = (LoginEvent)inner_messages[1];
            connected = loginResult.IsConnected;
            return loginResult.IsConnected;

        }
        public void Logoff()
        {
            LogoffAction action = new LogoffAction();
            PersonalMail.SendMessage(action.ToString());
        }
        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            var message_line = e.Message;
            var message_list = parser.ToMessagesList(message_line);
            if (connected)
            {
                foreach (var message in message_list)
                {
                    ///recieve ping and not send back
                    if (message.EventName != "Ping")
                    {
                        Server.Mail.PostMessage(message);
                    }
                }
            }
            else
            {
                foreach (var message in message_list)
                {
                    inner_messages.Add(message);
                }
                response.Set();
            }
        }

        protected override void WorkCycle()
        {
            int iterator = 0;
            while (true)
            {
                if (Cts.Token.IsCancellationRequested)
                {
                    telepasu.log("Asterisk disconected");
                    return;
                }
                iterator++;
                if (iterator == 50)
                {
                    iterator = 0;
                    ///Send ping
                    var action = new PingAction();
                    PersonalMail.SendMessage(action.ToString());
                }
                List<ServerMessage> messages = Server.Mail.GrabMessages(this);
                foreach (var message in messages)
                {
                    PersonalMail.SendMessage(message.ToString());

                }
                Cts.Token.WaitHandle.WaitOne(75);
            }
            telepasu.log("Asterisk thread stopped");
        }

        protected override void Disconnected(object sender, MessageArgs e)
        {
            telepasu.log("Asterisk thread connection lost");
            Cts.Cancel();
        }
    }
}
