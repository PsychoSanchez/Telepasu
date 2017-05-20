using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxy.Engine;
using Proxy.Messages.API.Admin;
using Proxy.Messages.API.SystemCalls;

namespace Proxy.ServerEntities.NativeModule
{
    class MethodCallerNative : NativeModule
    {
        private readonly ProxyEngine _engine;

        public MethodCallerNative(ProxyEngine engine)
        {
            _engine = engine;
        }

        public void HandleSystemCalls()
        {
            MessagesReady.WaitOne(5000);
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
            var action = (LocalDbLoginMessage) message;
            var user = _engine.LocalDb.GetUser(action.Login, action.Secret);

            action.Sender.OnLogin();
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
            else if(action.Type == "LocalDB")
            {
                
            }
        }
    }
}
