using System;
using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    class LoginEvent : AsteriskMessage
    {
        public LoginEvent(string _message) : base(_message)
        {
            ActionID = Helper.GetValue(_message, "ActionID: ");
            var message = Helper.GetValue(_message, "Message: ");
            if (message == "Authentication accepted")
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
            }
        }

        public string ActionID { get; private set; }
        public bool IsConnected { get; }
        public override string ToApi()
        {
            throw new NotImplementedException();
        }
    }
}
