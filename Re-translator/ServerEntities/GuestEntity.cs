using Proxy.Helpers;
using Proxy.Messages.API;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using Proxy.Engine;

namespace Proxy.ServerEntities.Application
{
    class AuthEventArgs : EventArgs
    {
        public bool Authentificated;
        public string Message;
        public EntityManager Client;
        public AuthEventArgs(bool authentificated, string message, EntityManager client)
        {
            Authentificated = authentificated;
            Message = message;
            Client = client;
        }
        public AuthEventArgs(bool authentificated, string message)
        {
            Authentificated = authentificated;
            Message = message;
            Client = null;
        }
    }
    class GuestEntity : EntityManager
    {
        //private readonly ConnectionTimer _timer;
        private EntityManager _entity;
        private string _challenge;
        // TODO: Create async await functions instead of events
        public event EventHandler<AuthEventArgs> AuthorizationOver;

        public GuestEntity(TcpClient tcp, int timeout) : base(tcp)
        {
            //_timer = new ConnectionTimer(timeout);
        }

        public void BeginAutorization()
        {
            Listen();
            //_timer.Wait();
            //return _entity;
        }

        private void OnAuthorizationOver(string message, EntityManager entity)
        {
            var e = new AuthEventArgs(true, message, entity);
            AuthorizationOver?.Invoke(this, e);
        }
        private void OnAuthorizationOver(string message)
        {
            var e = new AuthEventArgs(false, message);
            AuthorizationOver?.Invoke(this, e);
        }
        protected override void Disconnected(object sender, MessageArgs e)
        {
            telepasu.log(UserName + " disconnected");
        }

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
                        var username = Helper.GetValue(e.Message, "UserName: ");
                        if (username != null && username == "")
                        {
                            username = Helper.GetValue(e.Message, "Username: ");
                        }
                        var pwd = Helper.GetValue(e.Message, "Key");
                        if (pwd == "")
                        {
                            pwd = Helper.GetValue(e.Message, "secret");
                        }
                        var actionId = Helper.GetValue(e.Message, "ActionID: ");

                        try
                        {
                            //Authentificated = _challenge != null ? SocketServer.MailPost.DB.Authentificate(username, pwd, _challenge) : SocketServer.MailPost.DB.Authentificate(username, pwd);
                            Authentificated = true;
                        }
                        catch (Exception exception)
                        {
                            Authentificated = false;
                            telepasu.exc(exception);
                            StopListen();
                        }

                        if (Authentificated)
                        {
                            StopListen();
                            UserName = username;
                            var type = Helper.GetValue(e.Message, "Type: ");
                            EntityManager app;
                            switch (type)
                            {
                                case "Light":
                                    break;
                                case "Admin":
                                    app = new AdminEntity(PersonalMail);
                                    OnAuthorizationOver("Welcome", app);
                                    break;
                                default:
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
                            OnAuthorizationOver("Authentification failed");
                        }
                        //_timer.StopWait();
                        break;
                    case "Auth":
                        telepasu.log("Auth not implemented yet");
                        Shutdown();
                        OnAuthorizationOver("Auth not implemented yet");
                        //_timer.StopWait();
                        break;
                    case "Ping":
                        var pingAction = new PingEvent();
                        if (message.ActionId != "")
                        {
                            pingAction.ActionId = message.ActionId;
                        }
                        PersonalMail.SendMessage(pingAction.ToString());
                        return;
                    default:
                        Shutdown();
                        OnAuthorizationOver("Unknown message recieved");
                        //_timer.StopWait();
                        break;
                }
            }
            return;
        }

        protected override void WorkAction()
        {
        }
    }
}
