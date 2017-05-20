using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Proxy.ServerEntities;
using Proxy.ServerEntities.Application;
using Proxy.ServerEntities.NativeModule;
using Proxy.LocalDB;

namespace Proxy.Engine
{
    class ProxyEngine
    {
        private const string ModuleName = "#(Engine) ";
        public static readonly MailPost MailPost = new MailPost();
        private MethodCallerNativeModule _innerEvents;
        private bool _working = true;
        private SocketServer _listener;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly LocalDbCommandDispatcher _localDb = new LocalDbCommandDispatcher(); 

        public ProxyEngine()
        {
        }

        public void Start()
        {
            telepasu.log(ModuleName + "Proxy engine started. Welcome to Telepasu 2.0.");
            ThreadPool.QueueUserWorkItem(InnerEventsDoWork, this);
            ThreadPool.QueueUserWorkItem(ListenerDoWork);

            _localDb.ConnectLocalDb();
            _localDb.InitTables();
        }
        public void Stop()
        {
            _cts.Cancel();
            _listener.Stop();
        }
        public async void ConnectNativeModule(string type, ConnectionData data)
        {
            switch (type)
            {
                case "Asterisk":
                    Socket socket = _listener.GetSocket();
                    try
                    {
                        IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(data.Ip), Convert.ToInt32(data.Port));
                        socket.Connect(endpoint);
                        AsteriskEntity asterisk = new AsteriskEntity(socket);
                        if (await asterisk.Login(data.Username, data.Password))
                        {
                            telepasu.log(ModuleName + "#Asterisk connected...");
                            ProxyEngine.MailPost.AddNativeModule("AsteriskServer1", asterisk);
                            ThreadPool.QueueUserWorkItem(AsteriskThread, asterisk);
                            return;
                        }

                        telepasu.log(ModuleName + "#Failed to connect asterisk...");
                    }
                    catch (SocketException e)
                    {
                        if (e.SocketErrorCode != SocketError.TimedOut) return;

                        telepasu.log("Сервер недоступен");
                        throw new Exception("Сервер недоступен");
                    }
                    break;
                case "LocalDB":
                    {
                        _localDb.ConnectLocalDb();
                        _localDb.InitTables();
                    }
                    break;
                default:
                    break;
            }
        }

        private void AsteriskThread(object state)
        {
            EntityManager asterisk = (EntityManager)state;
            ProxyEngine.MailPost.Subscribe(asterisk, NativeModulesTags.Asterisk + NativeModulesTags.Incoming);
            asterisk.StartWork();
        }

        private void InnerEventsDoWork(object state)
        {
            telepasu.log(ModuleName + "Inner events initialized");
            _innerEvents = new MethodCallerNative(this);
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
