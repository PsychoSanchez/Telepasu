using Proxy.Helpers;
using Proxy.ServerEntities.Asterisk;
using Proxy.ServerEntities.Application;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Proxy.Engine;
using Proxy.Messages.API.Light;

namespace Proxy.ServerEntities
{
    public abstract class EntityManager
    {
        private const string ModuleName = "#(Entity Manager) ";
        public string UserName = "guest";
        protected readonly CancellationTokenSource Cts = new CancellationTokenSource();
        protected readonly AutoResetEvent MessagesReady = new AutoResetEvent(false);
        protected UserRole Role = UserRole.Guest;
        protected bool Authentificated = false;
        protected SocketMail PersonalMail;
        protected readonly MessagesParser Parser = new MessagesParser();
        protected readonly ConcurrentQueue<ServerMessage> Mailbox = new ConcurrentQueue<ServerMessage>();

        protected EntityManager()
        {
        }

        protected EntityManager(SocketMail mail)
        {
            PersonalMail = mail;
            PersonalMail.MessageRecieved += ObtainMessage;
            PersonalMail.Disconnected += Disconnected;
            PersonalMail.InitReciever();
        }

        protected EntityManager(TcpClient tcp)
        {
            PersonalMail = new SocketMail(tcp);
            PersonalMail.MessageRecieved += ObtainMessage;
            PersonalMail.Disconnected += Disconnected;
        }
        protected EntityManager(Socket client)
        {
            PersonalMail = new SocketMail(client);
            PersonalMail.MessageRecieved += ObtainMessage;
            PersonalMail.Disconnected += Disconnected;
        }
        protected void Listen()
        {
            PersonalMail.InitReciever();
            PersonalMail.SendMessage("Asterisk Call Manager/" + ProxyEngine.MailPost.AsteriskVersion + Helper.LINE_SEPARATOR + Helper.LINE_SEPARATOR);
        }
        protected virtual void TimeOut(object sender, EventArgs e)
        {
            Shutdown();
        }

        protected abstract void ObtainMessage(object sender, MessageArgs e);

        protected virtual void Disconnected(object sender, MessageArgs e)
        {
            telepasu.log(ModuleName + "User " + UserName + " Disconnected: " + e.Message);
            Shutdown();
        }

        protected virtual void WorkAction()
        {
            var messages = GrabMessages();
            foreach (ServerMessage message in messages)
            {
                PersonalMail.SendMessage(message.ToString());
            }
        }

        protected List<ServerMessage> GrabMessages()
        {
            var messages = new List<ServerMessage>();
            while (!Mailbox.IsEmpty)
            {
                ServerMessage item;
                if (Mailbox.TryDequeue(out item))
                {
                    messages.Add(item);
                }
            }
            return messages;
        }
        public void StartWork()
        {
            while (true)
            {
                MessagesReady.WaitOne(5000);
                if (Cts.Token.IsCancellationRequested)
                {
                    telepasu.log(ModuleName + UserName + " disconnected");
                    return;
                }
                WorkAction();
                Cts.Token.WaitHandle.WaitOne(10);
            }
        }

        public void StopWork()
        {
            Cts.Cancel();
        }

        public void SendMesage(ServerMessage message)
        {
            Mailbox.Enqueue(message);
            MessagesReady.Set();
        }

        protected void StopListen()
        {
            telepasu.log(ModuleName + "Shutdown called at " + UserName);
            if (PersonalMail == null) return;
            PersonalMail.StopListen();
            PersonalMail.MessageRecieved -= ObtainMessage;
            PersonalMail.Disconnected -= Disconnected;
        }

        protected void Shutdown()
        {
            telepasu.log(ModuleName + "Shutdown called at " + UserName);
            ShutdownConnection();
        }

        public void Shutdown(string message)
        {
            telepasu.log(ModuleName + "Shutdown called at " + UserName + " by reason: " + message);
            PersonalMail.SendMessage(message);
            ShutdownConnection();
        }

        private void ShutdownConnection()
        {
            if (PersonalMail != null)
            {
                PersonalMail.Disconnect();
                PersonalMail.MessageRecieved -= ObtainMessage;
                PersonalMail.Disconnected -= Disconnected;
            }
            PersonalMail = null;
            StopWork();
        }
        protected void Disconnect()
        {
            var subs = ProxyEngine.MailPost.GetSubscribtions(this);
            foreach (string sub in subs)
            {
                telepasu.log(ModuleName + UserName + " sub removed: " + sub);
                ProxyEngine.MailPost.Unsubscrive(this, sub);
            }
            PersonalMail.SendJsonMessage(new Disconnected()
            {
                Status = 200,
                Message = "Thanks for all the fish!"
            });
            Shutdown();
        }
    }
}
