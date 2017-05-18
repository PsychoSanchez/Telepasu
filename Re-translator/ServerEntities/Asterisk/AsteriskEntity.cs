using Proxy.Helpers;
using Proxy.ServerEntities.Asterisk;
using Proxy.ServerEntities.Messages;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Proxy.Engine;

namespace Proxy.ServerEntities.Application
{
    public class AsteriskEntity : EntityManager
    {
        public string Version;
        private readonly MessagesParser _parser = new MessagesParser();
        private readonly AutoResetEvent _response = new AutoResetEvent(false);
        private readonly List<AsteriskMessage> _innerMessages = new List<AsteriskMessage>();

        public AsteriskEntity(Socket client) : base(client)
        {
            UserName = "Asterisk Native Module";
        }
        public bool Connected { get; set; }

        public async Task<bool> Login(string login, string password)
        {
            _innerMessages.Clear();

            // First step - Send Challenge to get prefix for MD5 hash
            AsteriskAction action = new ChallengeAction();
            var message = action.ToString();
            PersonalMail.SendMessage(message);
            if (!_response.WaitOne(5000))
            {
                return false;
            }

            // Second step - Encrypt user pwd and send data to server
            var challengeResult = (ChallengeEvent)_innerMessages[0];
            ProxyEngine.MailPost.AsteriskVersion = challengeResult.Version;
            string key = Encryptor.CalculateMD5Hash(challengeResult.Challenge + password);


            action = new LoginAction(login, "MD5", key, null);
            message = action.ToString();
            PersonalMail.SendMessage(message);
            if (!_response.WaitOne(5000))
            {
                return false;
            }
            var loginResult = (LoginEvent)_innerMessages[1];
            Connected = loginResult.IsConnected;
            return loginResult.IsConnected;

        }
        public void Logoff()
        {
            LogoffAction action = new LogoffAction();
            PersonalMail.SendMessage(action.ToString());
        }
        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            var messageLine = e.Message;
            var messageList = _parser.ToMessagesList(messageLine);
            if (Connected)
            {
                foreach (var message in messageList)
                {
                    // recieve ping and not send back
                    if (message.EventName != "Ping")
                    {
                        message.Tag = NativeModulesTags.Asterisk;
                        ProxyEngine.MailPost.PostMessage(message);
                    }
                }
            }
            else
            {
                foreach (var message in messageList)
                {
                    _innerMessages.Add(message);
                }
                _response.Set();
            }
        }

        protected override void WorkAction()
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
                List<ServerMessage> messages = GrabMessages();
                foreach (ServerMessage message in messages)
                {
                    PersonalMail.SendMessage(message.ToString());

                }
                Cts.Token.WaitHandle.WaitOne(75);
            }
        }

        private void SendPing()
        {
            
        }

        protected override void Disconnected(object sender, MessageArgs e)
        {
            telepasu.log("Asterisk thread connection lost");
            Cts.Cancel();
        }
    }
}
