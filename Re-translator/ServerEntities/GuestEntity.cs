using Proxy.Helpers;
using Proxy.Messages.API;
using System;
using System.Net.Sockets;
using Proxy.Engine;
using Proxy.Messages.API.Admin;
using Proxy.Messages.API.Light;
using Proxy.Messages.API.SystemCalls;

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
        private string _challenge;
        // TODO: Create async await functions instead of events
        public event EventHandler<AuthEventArgs> AuthorizationOver;
        private string _loginMessage = null;

        public GuestEntity(TcpClient tcp, int timeout) : base(tcp)
        {
        }

        protected override void Disconnected(object sender, MessageArgs e)
        {
            OnAuthorizationOver(ResponseMessages.LOGIN_FAILED, 408); // 409 - Conflict
            Shutdown();
        }

        public void BeginAutorization()
        {
            Listen();
        }

        /// <summary>
        /// Method called when authorization successfull
        /// </summary>
        /// <param name="username"></param>
        /// <param name="entity"></param>
        private void OnAuthorizationOver(string username, EntityManager entity)
        {
            var e = new AuthEventArgs(username, entity);
            AuthorizationOver?.BeginInvoke(this, e, ar => { }, null);
        }

        /// <summary>
        /// Method called when user kicked by som reason
        /// </summary>
        /// <param name="username"></param>
        /// <param name="status"></param>
        private void OnAuthorizationOver(string username, int status)
        {
            var e = new AuthEventArgs(username);
            PersonalMail.SendJsonMessage(new Disconnected
            {
                Message = username,
                Status = status
            });
            AuthorizationOver?.BeginInvoke(this, e, ar => { }, null);
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
                            ProxyEngine.MailPost.PostMessage(new LocalDbLoginMessage(this)
                            {
                                Login = username,
                                Challenge = _challenge,
                                Secret = pwd,
                                Role = Helper.GetValue(_loginMessage, "Type: "),
                                Sender = this,
                                Action = "Login"
                            });
                        }
                        else
                        {
                            OnAuthorizationOver(ResponseMessages.DDOS_RESPONSE, 409); // 409 - Conflict
                            Shutdown();
                        }

                        break;
                    case "Auth":
                        telepasu.log("Auth not implemented yet");
                        OnAuthorizationOver(ResponseMessages.NOT_IMPLEMENTED, 423); // 423 - Locked
                        Shutdown();
                        break;
                    case "Ping":
                        var pingAction = new PingEvent(message.ActionId);
                        PersonalMail.SendMessage(pingAction.ToString());
                        return;
                    default:
                        OnAuthorizationOver(ResponseMessages.UNKNOWN_MESSAGE_RECIEVED, 500); // Internal error
                        Shutdown();
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
                var userName = Helper.GetValue(_loginMessage, "username");

                var type = Helper.GetValue(_loginMessage, "Type: ");
                EntityManager app;
                switch (type)
                {
                    case "Light":
                        app = new LightEntity(PersonalMail)
                        {
                            UserName = userName
                        };
                        OnAuthorizationOver(userName, app);
                        break;
                    case "Admin":
                        try
                        {
                            app = new AdminEntity(PersonalMail)
                            {
                                UserName = userName
                            };
                        }
                        catch (Exception e)
                        {
                            return;
                        }
                        OnAuthorizationOver(userName, app);
                        break;
                    default:
                        app = new HardEntity(PersonalMail, actionId)
                        {
                            UserName = UserName
                        };
                        ProxyEngine.MailPost.AddApplication(app.UserName, app);
                        ProxyEngine.MailPost.Subscribe(app, NativeModulesTags.Asterisk);
                        OnAuthorizationOver("Welcome", app);
                        break;
                }
            }
            else
            {
                OnAuthorizationOver(ResponseMessages.LOGIN_FAILED, message.Status);
            }
        }

        protected override void WorkAction()
        {
        }
    }
}
