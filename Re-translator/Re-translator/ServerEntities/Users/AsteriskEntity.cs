using Proxy.Helpers;
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
        private Parser parser = new Parser();
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
            ///First step - Send Challenge to get prefix for MD5 hash
            AsteriskAction action = new ChallengeAction();
            var message = action.ToString();
            personal_mail.SendMessage(message);
            if (!response.WaitOne(5000))
            {
                return false;
            }

            ///Second step - Encrypt user pwd and send data to server
            var challengeResult = (ChallengeEvent)inner_messages[0];
            string key = Encryptor.CalculateMD5Hash(challengeResult.Challenge + password);


            action = new LoginAction(login, "MD5", key, null);
            message = action.ToString();
            personal_mail.SendMessage(message);
            if (!response.WaitOne(5000))
            {
                return false;
            }
            var loginResult = (LoginEvent)inner_messages[1];
            connected = loginResult.IsConnected;
            return loginResult.IsConnected;

        }
        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            var message_line = e.Message;
            var message_list = parser.ToMessagesList(message_line);
            if (connected)
            {
                foreach (var message in message_list)
                {
                    Console.WriteLine(message.ToString());
                    Server.Mail.SendMessage(message);
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
            while (true)
            {
                if (cts.Token.IsCancellationRequested)
                {
                    Console.WriteLine("Asterisk disconected");
                    return;
                }
                List<ServerMessage> messages = Server.Mail.GrabMessages(this);
                foreach (var message in messages)
                {
                    personal_mail.SendMessage(message.ToString());
                }
                cts.Token.WaitHandle.WaitOne(100);
            }
        }

        protected override void Disconnected(object sender, MessageArgs e)
        {
            Console.WriteLine("disconnected from Asterisk ");
            cts.Cancel();
        }
    }
}
