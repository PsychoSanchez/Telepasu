using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxy.Engine;
using Proxy.Messages.API.Admin;

namespace Proxy.ServerEntities.NativeModule
{
    class MethodCallerNativeModule: NativeModule
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
            foreach (MethodCall message in messages)
            {
                //telepasu.log();
            }
        }
    }
}
