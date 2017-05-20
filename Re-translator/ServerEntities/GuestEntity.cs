using Proxy.Helpers;
using Proxy.Messages.API;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using Proxy.Engine;
using Proxy.Messages.API.Admin;
using Proxy.Messages.API.SystemCalls;
using Proxy.ServerEntities.Messages;
using PingEvent = Proxy.Messages.API.PingEvent;

namespace Proxy.ServerEntities.Application
{
    class AuthEventArgs : EventArgs
    {
        public readonly bool Authentificated;
        public string Message;
        public readonly EntityManager Client;
        public AuthEventArgs(string message, EntityManager client)
        {
            Authentificated = true;
            Message = message;
            Client = client;
        }
        public AuthEventArgs(string message)
        {
            Authentificated = false;
            Message = message;
            Client = null;
        }
    }
    class GuestEntity : EntityManager
    {
        private readonly ConnectionTimer _timer;
        private string _challenge;
        // TODO: Create async await functions instead of events
        public event EventHandler<AuthEventArgs> AuthorizationOver;
        private string _loginMessage = null;

        public GuestEntity(TcpClient tcp, int timeout) : base(tcp)
        {
            _timer = new ConnectionTimer(timeout);
            _timer.TimeOut += _timer_TimeOut;
        }

        private void _timer_TimeOut(object sender, EventArgs e)
        {
            Shutdown();
            OnAuthorizationOver("Fock u", 408); // 409 - Conflict
        }

        public void BeginAutorization()
        {
            Listen();
            _timer.Start();
        }

        private void OnAuthorizationOver(string message, EntityManager entity)
        {
            var e = new AuthEventArgs(message, entity);
            AuthorizationOver?.BeginInvoke(this, e, ar => {}, null);
        }
        private void OnAuthorizationOver(string message, int status)
        {
            var e = new AuthEventArgs(message);
            AuthorizationOver?.BeginInvoke(this, e, ar => {}, null);
        }
        protected override void Disconnected(object sender, MessageArgs e)
        {
            telepasu.log(UserName + " disconnected");
        }

        /// <summary>
        /// Recieves messages from client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            var actions = Parser.ToActionList(e.Message);
            foreach (AsteriskAction message in actions)
            {
                switch (message.Action)
                {
                    // TODO: Challenge dudos
                    case "Challenge":
                        _challenge = Encryptor.GenerateChallenge();
                        var asterAction = new Challenge(e.Message, _challenge);
                        PersonalMail.SendMessage(asterAction.ToString());
                        //_timer.Reset();
                        break;
                    case "Login":
                        if (_loginMessage == null)
                        {
                            _loginMessage = e.Message;
                            var username = Helper.GetValue(_loginMessage, "UserName: ");
                            if (username != null && username == "")
                            {
                                username = Helper.GetValue(_loginMessage, "Username: ");
                            }
                            var pwd = Helper.GetValue(_loginMessage, "Key");
                            if (pwd == "")
                            {
                                pwd = Helper.GetValue(_loginMessage, "secret");
                            }
                            ProxyEngine.MailPost.PostMessage(new LocalDbLoginMessage
                            {
                                Login =  username,
                                Secret = pwd,
                                Role = Helper.GetValue(_loginMessage, "Type: "),
                                Sender = this,
                                Action = "Login"
                            });
                        }
                        else
                        {
                            Shutdown();
                            OnAuthorizationOver("Fock u", 409); // 409 - Conflict
                        }

                        break;
                    case "Auth":
                        telepasu.log("Auth not implemented yet");
                        Shutdown();
                        OnAuthorizationOver("Auth not implemented yet", 423); // 423 - Locked
                        break;
                    case "Ping":
                        var pingAction = new PingEvent(message.ActionId);
                        PersonalMail.SendMessage(pingAction.ToString());
                        return;
                    default:
                        Shutdown();
                        OnAuthorizationOver("Unknown message recieved", 500); // Internal error
                        break;
                }
            }
        }

        /// <summary>
        /// OnLogin Message 
        /// Called from engine
        /// </summary>
        /// <param name="message"></param>
        public void OnLogin(AuthResponse message)
        {
            if (message.Status == 200)
            {
                StopListen();
                var actionId = Helper.GetValue(_loginMessage, "ActionID: ");

                var type = Helper.GetValue(_loginMessage, "Type: ");
                EntityManager app;
                switch (type)
                {
                    case "Light":
                        app = new LightEntity(PersonalMail);
                        OnAuthorizationOver("Welcome", app);
                        break;
                    case "Admin":
                        app = new AdminEntity(PersonalMail);
                        OnAuthorizationOver("Welcome", app);
                        break;
                    default:
                        UserName = Helper.GetValue(_loginMessage, "username");
                        app = new HardEntity(PersonalMail, actionId);
                        ProxyEngine.MailPost.AddApplication(app.UserName, app);
                        ProxyEngine.MailPost.Subscribe(app, NativeModulesTags.Asterisk);
                        OnAuthorizationOver("Welcome", app);
                        break;
                }
            }
            else
            {
                // TODO: Add authentification failed message
                OnAuthorizationOver("Authentification failed", message.Status);
            }
        }

        protected override void WorkAction()
        {
        }
    }
}
