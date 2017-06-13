using System;
using Newtonsoft.Json;
using Proxy.Engine;
using Proxy.Helpers;
using Proxy.Messages.API.Admin;
using Proxy.Messages.API.Light;
using Proxy.Messages.API.SystemCalls;
using Proxy.ServerEntities.Messages;

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
                case "DBGetContactsAction":
                    Console.WriteLine(e.Message);
                    break;
                case "AsteriskCall":
                    var callAction = JsonConvert.DeserializeObject<CallAction>(e.Message);
                    var originateAction = new OriginateAction(callAction.Exten + "/" + callAction.Number, "default",
                        callAction.Number, 1)
                    { Tag = NativeModulesTags.Asterisk + NativeModulesTags.Incoming };
                    ProxyEngine.MailPost.PostMessage(originateAction);
                    break;
                case "Subscribe":
                    var subPidor = JsonConvert.DeserializeObject<SubscribeMessage>(e.Message);
                    ProxyEngine.MailPost.PostMessage(new SubscribeMethod(this)
                    {
                        SubscribeTag = subPidor.Tag
                    });
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
