using System.Threading;
using Proxy.ServerEntities.NativeModule;

namespace Proxy.Engine
{
    class ProxyEngine
    {
        private const string ModuleName = "#(Engine) ";
        public static readonly MailPost MailPost = new MailPost();
        private MethodCallerNativeModule _innerEvents;
        private Methods _methods;
        private bool _working = true;
        private SocketServer _listener;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public ProxyEngine()
        {
        }

        public void Start()
        {
            telepasu.log(ModuleName + "Proxy engine started. Welcome to Telepasu 2.0.");
            ThreadPool.QueueUserWorkItem(InnerEventsDoWork, this);
            ThreadPool.QueueUserWorkItem(ListenerDoWork);
        }
        public void Stop()
        {
            _cts.Cancel();
            _listener.Stop();
        }

        private void InnerEventsDoWork(object state)
        {
            telepasu.log(ModuleName + "Inner events initialized");
            _innerEvents = new MethodCallerNativeModule(this);
            MailPost.AddNativeModule("Inner Calls", _innerEvents);
            while (!_cts.IsCancellationRequested)
            {
                _innerEvents.HandleSystemCalls();
            }
            telepasu.log(ModuleName + "System calls stopped");
            //this._methods;_
        }

        private void ListenerDoWork(object state)
        {
            _listener = new SocketServer();
            _listener.Init();
            telepasu.log(ModuleName + "Listener initialized");
            while (!_cts.IsCancellationRequested)
            {
                _listener.AcceptClient();
            }
            telepasu.log(ModuleName + "Listener stopped");
        }
    }
}
