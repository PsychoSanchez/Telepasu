using Proxy.ServerEntities.UserEntities;
using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;

namespace Proxy.ServerEntities
{
    public abstract class ServerEntity
    {
        protected CancellationTokenSource cts = new CancellationTokenSource();
        public ConcurrentQueue<string> MessageStack;
        protected UserRole role = UserRole.Guest;
        public string UserName = "guest";
        protected bool connectionStatus = false;
        protected SocketMail client_mail;
        public int ThreadNumber;

        public ServerEntity(Socket _client)
        {
            client_mail = new SocketMail(_client);
            client_mail.MessageRecieved += ObtainMessage;
        }
        
        protected abstract void ObtainMessage(object sender, MessageArgs e);
        protected abstract void WorkCycle();
        public void StartWork()
        {
            WorkCycle();
        }
        public void Stop()
        {
            Console.WriteLine("User disconnecting");
            cts.Cancel();
        }
    }
}
