using Proxy.Helpers;
using Proxy.ServerEntities.Asterisk;
using Proxy.ServerEntities.Users;
using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;

namespace Proxy.ServerEntities
{
    public abstract class UserManager
    {
        protected readonly CancellationTokenSource Cts = new CancellationTokenSource();
        public ConcurrentQueue<string> MessageStack;
        protected UserRole Role = UserRole.Guest;
        public string UserName = "guest";
        protected bool Authentificated = false;
        protected SocketMail PersonalMail;
        public int ThreadNumber;
        protected readonly MessagesParser Parser = new MessagesParser();

        public UserManager()
        {
        }
        public UserManager(Socket client)
        {
            PersonalMail = new SocketMail(client);
            PersonalMail.MessageRecieved += ObtainMessage;
            PersonalMail.Disconnected += Disconnected;
        }
        protected void Listen(Socket client)
        {
            PersonalMail = new SocketMail(client);
            PersonalMail.MessageRecieved += ObtainMessage;
            PersonalMail.Disconnected += Disconnected;
            PersonalMail.SendMessage("Asterisk Call Manager/" + Server.Mail.AsteriskVersion + Helper.LINE_SEPARATOR + Helper.LINE_SEPARATOR);
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
            telepasu.log("Shutdown called at " + UserName);
            Cts.Cancel();
            PersonalMail.Disconnect();
            PersonalMail.MessageRecieved -= ObtainMessage;
            PersonalMail.Disconnected -= Disconnected;
            PersonalMail.TimeOut -= TimeOut;
        }
    }
}
