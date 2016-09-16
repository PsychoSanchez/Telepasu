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
        protected bool authentificated = false;
        protected SocketMail personal_mail;
        public int ThreadNumber;

        public UserManager()
        {
        }
        public UserManager(Socket _client)
        {
            Listen(_client);
        }
        protected void Listen(Socket _client)
        {
            personal_mail = new SocketMail(_client);
            personal_mail.MessageRecieved += ObtainMessage;
            personal_mail.Disconnected += Disconnected;
        }
        protected virtual void TimeOut(object sender, EventArgs e)
        {
            Shutdown();
        }

        protected abstract void ObtainMessage(object sender, MessageArgs e);
        protected abstract void Disconnected(object sender, MessageArgs e);
        protected abstract void WorkCycle();
        public void StartWork()
        {
            WorkCycle();
        }
        public void Shutdown()
        {
            telepasu.log("User disconnecting");
            cts.Cancel();
            personal_mail.Disconnect();
            personal_mail.MessageRecieved -= ObtainMessage;
            personal_mail.Disconnected -= Disconnected;
            personal_mail.TimeOut -= TimeOut;
        }
    }
}
