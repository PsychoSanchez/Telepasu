using Newtonsoft.Json;
using Proxy.Engine;
using Proxy.Helpers;
using Proxy.Messages.API.Admin;
using Proxy.Messages.API.Light;

namespace Proxy.ServerEntities.Application
{
    internal class AdminEntity : EntityManager
    {
        public AdminEntity(SocketMail mail) : base(mail)
        {
            Role = UserRole.Admin;
            PersonalMail.IsApi = true;
            PersonalMail.SendApiMessage(JsonConvert.SerializeObject(new AuthResponse
            {
                Status = 200,
                Action = "Login",
                Message = ResponseMessages.WELCOME_MESSAGE
            }));
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
                case "Add Module":
                    var command = JsonConvert.DeserializeObject<AddModuleCommand>(e.Message);
                    ProxyEngine.MailPost.PostMessage(command);
                    break;
                case "Ping":
                    PersonalMail.SendApiMessage(JsonConvert.SerializeObject(new Ping()));
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
