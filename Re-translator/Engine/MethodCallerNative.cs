using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Proxy.Engine;
using Proxy.LocalDB.UsersTable;
using Proxy.Messages.API.Admin;
using Proxy.Messages.API.SystemCalls;
using Proxy.ServerEntities.Application;
using Proxy.ServerEntities.Module;
using Newtonsoft.Json;

namespace Proxy.ServerEntities.NativeModule
{
    class MethodCallerNative : NativeModule
    {
        private readonly ProxyEngine _engine;

        private const string ModuleName = "#(Method Caller) ";
        public MethodCallerNative(ProxyEngine engine) : base()
        {
            _engine = engine;
        }

        struct Response
        {
            public string Action;
            public string Status;
            public string Answer;
        }

        public void HandleSystemCalls()
        {
            telepasu.log(ModuleName + "Waiting message");
            MessagesReady.WaitOne();
            telepasu.log(ModuleName + "Message recieved");
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
                        Task.Run(() =>
                        {
                            GetModulesList(message);
                        });
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
                    case "Get White List":
                        Task.Run(() =>
                        {
                            message.Sender.SendMesage(JsonConvert.SerializeObject(
                                new Response()
                                {
                                    Action = "Get White List",
                                    Status = "true",
                                    Answer = JsonConvert.SerializeObject(ProxyEngine.LocalDb.GetWhiteList())
                                }));
                        });
                        break;
                    case "Add White List":
                        Task.Run(() =>
                        {
                            var addIp = (AddWhiteListMethod)message;
                            ProxyEngine.LocalDb.AddWhiteListItem(addIp.Address);
                            message.Sender.SendMesage(JsonConvert.SerializeObject(message));
                        });
                        break;
                    case "Remove White List":
                        Task.Run(() =>
                        {
                            var remove = (RemoveWhiteListMethod)message;
                            ProxyEngine.LocalDb.DeleteWhiteList(remove.Address);
                            message.Sender.SendMesage(JsonConvert.SerializeObject(message));
                        });
                        break;
                }
            }
        }

        private void StartAuth(MethodCall message)
        {
            var action = (LocalDbLoginMessage)message;
            var user = ProxyEngine.LocalDb.GetUser(action.Login, action.Secret, action.Challenge);
            var data = user.Data as Users;
            var response = new AuthResponse();
            if (data != null)
            {
                var isAdmin = String.Equals(action.Role, data.Role, StringComparison.CurrentCultureIgnoreCase);
                //response.Status = isAdmin ? 200 : 404;
                response.Status = 200;
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
            switch (action.Type)
            {
                case "Asterisk":
                    _engine.ConnectNativeModule(action.Type, new ConnectionData()
                    {
                        Ip = action.Ip,
                        Password = action.Pwd,
                        Port = action.Port,
                        Username = action.Username
                    }, message.Sender, action);
                    break;
                case "LocalDB":

                    break;
                case "Module":
                    _engine.ConnectModule(action);
                    break;
            }
        }

        private void Subscribe(MethodCall message)
        {
            var action = message as SubscribeMethod;
            if (action != null)
            {
                ProxyEngine.MailPost.Subscribe(action.Sender, action.SubscribeTag);
            }
            action?.Sender.SendMesage(JsonConvert.SerializeObject(message));
        }

        private void GetModulesList(MethodCall message)
        {
            var modules = ProxyEngine.MailPost.GetConnectedModules();
            message.Sender.SendMesage(JsonConvert.SerializeObject(new GetModulesMessage()
            {
                Modules = modules
            }));
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
