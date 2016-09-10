using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.ServerEntities.UserEntities
{
    class AdminEntity : ServerEntity
    {
        public AdminEntity(Socket _client) : base(_client)
        {
            role = UserRole.Admin;
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            Console.WriteLine("Message recieved");
        }

        protected override void WorkCycle()
        {
            while (true)
            {
                if (cts.Token.IsCancellationRequested)
                {
                    Console.WriteLine("User disconnect");
                    return;
                }
                List<ServerMessage> messages = Server.Mail.GrabMessages(this);
                foreach(var message in messages)
                {
                    client_mail.SendMessage(message.ToString());
                }
                //Console.WriteLine("Thread working");
                cts.Token.WaitHandle.WaitOne(300);
            }
        }
    }
}
