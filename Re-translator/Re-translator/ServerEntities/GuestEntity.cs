using Proxy.Helpers;
using Proxy.Messages.API;
using System;
using System.Net.Sockets;

namespace Proxy.ServerEntities.Users
{
    class GuestEntity : UserManager
    {
        readonly Socket _client;
        readonly ConnectionTimer _timer;
        UserManager _user;
        string _challenge;
        public GuestEntity(Socket client, int timeout) : base()
        {
            this._client = client;
            _timer = new ConnectionTimer(timeout);
        }

        public UserManager StartAutorization()
        {
            Listen(_client);
            _timer.Wait();
            return _user;
        }
        protected override void Disconnected(object sender, MessageArgs e)
        {
            telepasu.log(UserName + " disconnected");
            return;
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            var actions = Parser.ToActionList(e.Message);
            foreach (var message in actions)
            {
                switch (message.Action)
                {
                    case "Challenge":
                        _challenge = Encryptor.GenerateChallenge();
                        var asterAction = new Challenge(e.Message, _challenge);
                        PersonalMail.SendMessage(asterAction.ToString());
                        _timer.Reset();
                        break;
                    case "Login":
                        var username = Helper.GetValue(e.Message, "UserName: ");
                        if (username != null && username == "")
                        {
                            username = Helper.GetValue(e.Message, "Username: ");
                        }
                        var pwd = Helper.GetValue(e.Message, "Key: ");
                        var actionId = Helper.GetValue(e.Message, "ActionID: ");

                        Authentificated = _challenge != null ? Server.Mail.DB.Authentificate(username, pwd, _challenge) : Server.Mail.DB.Authentificate(username, pwd);

                        if (Authentificated)
                        {
                            var type = Helper.GetValue(e.Message, "Type: ");
                            switch (type)
                            {
                                case "Light":
                                    break;
                                case "Admin":
                                    UserName = username;
                                    PersonalMail.StopListen();
                                    _user = new AdminUser(_client);
                                    break;
                                default:
                                    UserName = username;
                                    PersonalMail.StopListen();
                                    _user = new HardUser(_client, actionId);
                                    break;
                            } 
                        }
                        else
                        {
                            Shutdown();
                        }
                        _timer.StopWait();
                        break;
                    case "Auth":
                        telepasu.log("Auth not implemented yet");
                        Shutdown();
                        _timer.StopWait();
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
                        _timer.StopWait();
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
