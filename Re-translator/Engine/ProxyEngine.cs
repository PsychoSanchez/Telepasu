using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using Proxy.ServerEntities;
using Proxy.ServerEntities.Application;
using Proxy.ServerEntities.NativeModule;
using Proxy.LocalDB;
using Proxy.Messages.API.Admin;
using Proxy.ServerEntities.Module;

namespace Proxy.Engine
{
    class ProxyEngine
    {
        private const string ModuleName = "#(Engine) ";
        public static readonly MailPost MailPost = new MailPost();
        private MethodCallerNative _innerEvents;
        private bool _working = true;
        private SocketServer _listener;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        public readonly LocalDbCommandDispatcher LocalDb = new LocalDbCommandDispatcher();

        public ProxyEngine()
        {
        }

        public void Start()
        {
            telepasu.log(ModuleName + "Proxy engine started. Welcome to Telepasu 2.0.");
            ThreadPool.QueueUserWorkItem(InnerEventsDoWork, this);
            ThreadPool.QueueUserWorkItem(ListenerDoWork);

            LocalDb.ConnectLocalDb();
            LocalDb.InitTables();
        }
        public void Stop()
        {
            _cts.Cancel();
            _listener.Stop();
        }
        public async void ConnectNativeModule(string type, ConnectionData data, EntityManager sender)
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
                            sender.SendMesage(JsonConvert.SerializeObject(new AddModuleResponse()
                            {
                                Connected = true,
                                Status = "200",
                                Type = "Native Module"
                            }));
                            return;
                        }

                        sender.SendMesage(JsonConvert.SerializeObject(new AddModuleResponse()
                        {
                            Connected = false,
                            Status = "404",
                            Type = "Native Module"
                        }));
                        telepasu.log(ModuleName + "#Failed to connect asterisk...");
                    }
                    catch (SocketException e)
                    {
                        if (e.SocketErrorCode != SocketError.TimedOut) return;
                        sender.SendMesage(JsonConvert.SerializeObject(new AddModuleResponse()
                        {
                            Connected = false,
                            Status = "404",
                            Type = "Native Module"
                        }));
                        telepasu.log(ModuleName + "Сервер Asterisk недоступен");
                    }
                    break;
                case "LocalDB":
                    LocalDb.ConnectLocalDb();
                    LocalDb.InitTables();
                    break;
                default:
                    break;
            }
        }

        public void ConnectModule(AddModuleMethod action)
        {
            var tcp = new TcpClient();
            try
            {
                tcp.Connect(action.Ip, action.Port);
                var moduleEntity = new ModuleEntity(tcp);
                ThreadPool.QueueUserWorkItem(ModuleThread, moduleEntity);
                ProxyEngine.MailPost.AddModule("AnalyticsModule", moduleEntity, action);
                action.Sender.SendMesage(JsonConvert.SerializeObject(new AddModuleResponse()
                {
                    Connected = true,
                    Status = "200",
                    Type = "Module"
                }));
            }
            catch (Exception e)
            {
                action.Sender.SendMesage(JsonConvert.SerializeObject(new AddModuleResponse()
                {
                    Connected = false,
                    Status = "404",
                    Type = "Module"
                }));
                Console.WriteLine(e);
            }
        }

        private void ModuleThread(object state)
        {
            EntityManager module = (EntityManager)state;
            ProxyEngine.MailPost.Subscribe(module, NativeModulesTags.Asterisk);
            ProxyEngine.MailPost.Subscribe(module, NativeModulesTags.INNER_CALLS);
            ProxyEngine.MailPost.Subscribe(module, NativeModulesTags.DB);
            module.StartWork();
            ProxyEngine.MailPost.Unsubscribe(module);
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
