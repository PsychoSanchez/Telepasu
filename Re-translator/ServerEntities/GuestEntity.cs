using Proxy.Helpers;
using Proxy.Messages.API;
using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace Proxy.ServerEntities.Users
{
    class AuthEventArgs : EventArgs
    {
        public bool Authentificated;
        public string Message;
        public UserManager Client;
        public AuthEventArgs(bool authentificated, string message, UserManager client)
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
    class GuestEntity : UserManager
    {
        //private readonly ConnectionTimer _timer;
        private UserManager _user;
        private string _challenge;
        // TODO: Create async await functions instead of events
        public event EventHandler<AuthEventArgs> AuthorizationOver;

        public GuestEntity(TcpClient tcp, Socket client, int timeout) : base(tcp, client)
        {
            //_timer = new ConnectionTimer(timeout);
        }

        public void BeginAutorization()
        {
            Listen();
            //_timer.Wait();
            //return _user;
        }

        private void OnAuthorizationOver (string message, UserManager user)
        {
            var e = new AuthEventArgs(true, message, user);
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
                            Authentificated = _challenge != null ? Server.Mail.DB.Authentificate(username, pwd, _challenge) : Server.Mail.DB.Authentificate(username, pwd);
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
                            switch (type)
                            {
                                case "Light":
                                    break;
                                case "Admin":
                                    OnAuthorizationOver("Welcome", new AdminUser(PersonalMail));
                                    break;
                                default:
                                    OnAuthorizationOver("Welcome", new HardUser(PersonalMail, actionId));
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
                        if (message.ActionID != "")
                        {
                            pingAction.ActionID = message.ActionID;
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

        protected override void WorkCycle()
        {
        }
    }
}
