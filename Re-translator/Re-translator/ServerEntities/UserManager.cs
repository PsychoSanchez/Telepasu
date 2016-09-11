using Proxy.ServerEntities.Users;
using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;

namespace Proxy.ServerEntities
{
    public abstract class UserManager
    {
        protected CancellationTokenSource cts = new CancellationTokenSource();
        public ConcurrentQueue<string> MessageStack;
        protected UserRole role = UserRole.Guest;
        public string UserName = "guest";
        protected bool connectionStatus = false;
        protected SocketMail personal_mail;
        public int ThreadNumber;

        public UserManager(Socket _client)
        {
            personal_mail = new SocketMail(_client);
            personal_mail.MessageRecieved += ObtainMessage;
            personal_mail.Disconnected += Disconnected;
        }

        protected abstract void ObtainMessage(object sender, MessageArgs e);
        protected abstract void Disconnected(object sender, MessageArgs e);
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
