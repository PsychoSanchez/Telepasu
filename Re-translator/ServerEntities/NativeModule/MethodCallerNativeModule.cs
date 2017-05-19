using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxy.Engine;
using Proxy.Messages.API.Admin;

namespace Proxy.ServerEntities.NativeModule
{
    class MethodCallerNativeModule : NativeModule
    {
        private readonly ProxyEngine _engine;

        public MethodCallerNativeModule(ProxyEngine engine)
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
                    case "Add Module":
                        AddModule(message);
                        break;
                    case "Subscribe":
                        // TODO: Subscribe
                        // Call localDb method
                        // Check if user connected 
                        // Subscribe him
                        break;
                    case "Unsubscribe":
                        // TODO: Subscribe
                        // Call localDb method
                        // Check if user connected 
                        // Subscribe him
                        break;
                }
            }
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
