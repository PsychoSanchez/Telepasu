using Proxy.Helpers;
using Proxy.Messages.API;
using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace Proxy.ServerEntities.Users
{
    class GuestEntity : UserManager
    {
        private readonly ConnectionTimer _timer;
        private UserManager _user;
        private string _challenge;

        public GuestEntity(TcpClient tcp, Socket client, int timeout) : base(tcp, client)
        {
            _timer = new ConnectionTimer(timeout);
        }

        public UserManager StartAutorization()
        {
            Listen();
            _timer.Wait();
            return _user;
        }
        protected override void Disconnected(object sender, MessageArgs e)
        {
            telepasu.log(UserName + " disconnected");
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
                            PersonalMail.StopListen();
                        }

                        if (Authentificated)
                        {
                            PersonalMail.StopListen();
                            UserName = username;
                            var type = Helper.GetValue(e.Message, "Type: ");
                            switch (type)
                            {
                                case "Light":
                                    break;
                                case "Admin":
                                    _user = new AdminUser(PersonalMail);
                                    break;
                                default:
                                    _user = new HardUser(PersonalMail, actionId);
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
