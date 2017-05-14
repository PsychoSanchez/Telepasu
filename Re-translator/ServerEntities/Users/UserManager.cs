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
        public ConcurrentQueue<string> MessageStack;
        public string UserName = "guest";
        public int ThreadNumber;

        protected readonly CancellationTokenSource Cts = new CancellationTokenSource();
        protected UserRole Role = UserRole.Guest;
        protected bool Authentificated = false;
        protected readonly SocketMail PersonalMail;
        protected readonly MessagesParser Parser = new MessagesParser();

        protected UserManager()
        {
            
        }
        protected UserManager(SocketMail mail)
        {
            PersonalMail = mail;
            PersonalMail.MessageRecieved += ObtainMessage;
            PersonalMail.Disconnected += Disconnected;
            PersonalMail.InitReciever();
        }

        protected UserManager(TcpClient tcp, Socket client)
        {
            PersonalMail = new SocketMail(client, tcp);
            PersonalMail.MessageRecieved += ObtainMessage;
            PersonalMail.Disconnected += Disconnected;
        }
        protected UserManager(Socket client)
        {
            PersonalMail = new SocketMail(client);
            PersonalMail.MessageRecieved += ObtainMessage;
            PersonalMail.Disconnected += Disconnected;
        }
        protected void Listen()
        {
            PersonalMail.InitReciever();
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

        public void StopListen()
        {
            telepasu.log("Shutdown called at " + UserName);
            PersonalMail.StopListen();
            PersonalMail.MessageRecieved -= ObtainMessage;
            PersonalMail.Disconnected -= Disconnected;
            PersonalMail.TimeOut -= TimeOut;
        }
        public void Shutdown()
        {
            telepasu.log("Shutdown called at " + UserName);
            Cts.Cancel();
            PersonalMail.SendMessage("Disconnected");
            PersonalMail.Disconnect();
            PersonalMail.MessageRecieved -= ObtainMessage;
            PersonalMail.Disconnected -= Disconnected;
            PersonalMail.TimeOut -= TimeOut;
        }
    }
}
