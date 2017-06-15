using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Proxy.Engine;
using Proxy.LocalDB.UsersTable;
using Proxy.Messages.API.Admin;
using Proxy.Messages.API.SystemCalls;
using Proxy.ServerEntities.Application;
using Proxy.ServerEntities.Module;

namespace Proxy.ServerEntities.NativeModule
{
    class MethodCallerNative : NativeModule
    {
        private readonly ProxyEngine _engine;

        public MethodCallerNative(ProxyEngine engine) : base()
        {
            _engine = engine;
        }

        public void HandleSystemCalls()
        {
            MessagesReady.WaitOne();
            MessagesReady.Reset();
            var messages = GrabMessages();
            foreach (var serverMessage in messages)
            {
                var message = (MethodCall)serverMessage;
                switch (message.Action)
                {
                    case "Add Native Module":
                        Task.Run(() =>
                        {
                            AddModule(message);
                        });
                        break;
                    case "Add Module":
                        Task.Run(() =>
                        {
                            AddModule(message);
                        });
                        break;
                    case "Get Modules List":
                        break;
                    case "Get Applications List":
                        break;
                    case "Get Subscribers":
                        break;
                    case "Subscribe":
                        Task.Run(() =>
                        {
                            Subscribe(message);
                        });
                        // TODO: Subscribe
                        // Call localDb method
                        // Check if user connected 
                        // Subscribe him
                        break;
                    case "Unsubscribe":
                        Task.Run(() =>
                        {
                            Unsubscribe(message);
                        });
                        // TODO: Unsubscribe
                        // Call localDb method
                        // Check if user connected 
                        // Subscribe him
                        break;
                    case "Login":
                        Task.Run(() =>
                        {
                            StartAuth(message);
                        });
                        break;
                    case "Add User":
                        break;
                    case "Update User":
                        break;
                    case "Delete User":
                        break;
                    case "Update Password":
                        break;
                    case "Restart":
                        break;
                }
            }
        }

        private void StartAuth(MethodCall message)
        {
            var action = (LocalDbLoginMessage)message;
            var user = _engine.LocalDb.GetUser(action.Login, action.Secret, action.Challenge);
            var data = user.Data as Users;
            var response = new AuthResponse();
            if (data != null)
            {
                var isAdmin = String.Equals(action.Role, data.Role, StringComparison.CurrentCultureIgnoreCase);
                response.Status = isAdmin ? 200 : 404;
            }
            else
            {
                response.Status = user.Status;
            }
            ((GuestEntity)action.Sender).OnLogin(response);
        }

        private void AddModule(MethodCall message)
        {
            var action = (AddModuleMethod)message;
            if (action.Type == "Asterisk")
            {
                _engine.ConnectNativeModule(action.Type, new ConnectionData()
                {
                    Ip = action.Ip,
                    Password = action.Pwd,
                    Port = action.Port,
                    Username = action.Username
                });
            }
            else if (action.Type == "LocalDB")
            {

            }
            else if (action.Type == "Module")
            {
                _engine.ConnectModule(action);
            }
        }

        private void Subscribe(MethodCall message)
        {
            var action = message as SubscribeMethod;
            if (action != null)
            {
                ProxyEngine.MailPost.Subscribe(action.Sender, action.SubscribeTag);
            }
        }

        private void Unsubscribe(MethodCall message)
        {
            var action = message as SubscribeMethod;
            if (action != null)
            {
                ProxyEngine.MailPost.Unsubscribe(action.Sender, action.SubscribeTag);
            }
        }
    }
}
