using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxy.Engine;
using Proxy.LocalDB.UsersTable;
using Proxy.Messages.API.Admin;
using Proxy.Messages.API.SystemCalls;

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
            var messages = GrabMessages();
            foreach (var serverMessage in messages)
            {
                var message = (MethodCall)serverMessage;
                switch (message.Action)
                {
                    case "Add Native Module":
                        AddModule(message);
                        break;
                    case "Add Module":
                        AddModule(message);
                        break;
                    case "Get Modules List":
                        break;
                    case "Get Applications List":
                        break;
                    case "Subscribe":
                        // TODO: Subscribe
                        // Call localDb method
                        // Check if user connected 
                        // Subscribe him
                        break;
                    case "Unsubscribe":
                        // TODO: Unsubscribe
                        // Call localDb method
                        // Check if user connected 
                        // Subscribe him
                        break;
                    case "Login":
                        StartAuth(message);
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
            var user = _engine.LocalDb.GetUser(action.Login, action.Secret);
            var data = user.Data as Users;
            var response = new AuthResponse();
            if (data != null)
            {
                //var isAdmin = (action.Role.ToLower() == "admin" && data.Role == "admin");
                var isAdmin = (String.Equals(action.Role, data.Role, StringComparison.CurrentCultureIgnoreCase));
                response.Status = (isAdmin) ? 200 : 404;
            }
            else
            {
                response.Status = user.Status;
            }
            action.Sender.OnLogin(response);
        }

        private void AddModule(MethodCall message)
        {
            var action = (AddModuleCommand)message;
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
        }
    }
}
