using System;
using Newtonsoft.Json;
using Proxy.Helpers;
using Proxy.Messages.API.Admin;
using Proxy.Messages.API.Light;

namespace Proxy.ServerEntities.Application
{
    class LightEntity : EntityManager
    {
        public LightEntity(SocketMail mail) : base(mail)
        {
            Role = UserRole.User;
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
                case "Ping":
                    PersonalMail.SendApiMessage(JsonConvert.SerializeObject(new Ping
                    {
                        Action = "Ping"
                    }));
                    break;
                default:
                    break;
            }
        }

        //protected override void WorkAction()
        //{
        //    var action = Helper.GetJsonValue(e.Message, "action");
        //    switch (action)
        //    {
        //        case "Ping":
        //                PersonalMail.SendApiMessage(JsonConvert.SerializeObject(new Ping
        //                {
        //                    Action = "Ping"
        //                }));
        //                break;
        //        default:
        //                break;
        //    }
        //}
    }
}
