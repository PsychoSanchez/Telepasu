using System.Net.Sockets;
using Newtonsoft.Json;
using Proxy.Engine;
using Proxy.Helpers;
using Proxy.Messages.API.Admin;
using Proxy.Messages.API.Light;
using Proxy.Messages.API.Module;
using Proxy.Messages.API.SystemCalls;

namespace Proxy.ServerEntities.Module
{
    class ModuleEntity : EntityManager
    {
        public ModuleEntity(TcpClient client) : base(client)
        {
            Role = UserRole.Admin;
            PersonalMail.IsApi = true;
            
            PersonalMail.InitReciever();
            PersonalMail.SendJsonMessage(new WelcomeMessage());
        }
        protected override void Disconnected(object sender, MessageArgs e)
        {
            Cts.Cancel();
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            var action = Helper.GetJsonValue(e.Message, "action");
            switch (action)
            {
                case "Subscribe":
                    var subscribe = JsonConvert.DeserializeObject<SubscribeMessage>(e.Message);
                    ProxyEngine.MailPost.PostMessage(new SubscribeMethod(this)
                    {
                        SubscribeTag = subscribe.Tag
                    });
                    break;
                case "DBGetStatisticsResponse":

                    break;
                case "Unsubscribe":
                    var unsubscribe = JsonConvert.DeserializeObject<SubscribeMessage>(e.Message);
                    ProxyEngine.MailPost.PostMessage(new SubscribeMethod(this)
                    {
                        SubscribeTag = unsubscribe.Tag
                    });
                    break;
                case "Disconnect":
                    Disconnect();
                    break;
                default:
                    break;
            }
        }
    }
}
