using System.Text;

namespace Proxy.ServerEntities.Messages
{
    public class LoginAction : AsteriskAction
    {
        public override string Action
        {
            get
            {
                return "Login";
            }
        }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string AuthType { get; set; }
        public string Key { get; set; }
        public string Events { get; set; }

        public LoginAction()
        {
        }
        public LoginAction(string username, string pwd)
        {
            UserName = username;
            PassWord = pwd;
            AuthType = string.Empty;
            Key = string.Empty;
            Events = string.Empty;
        }
        public LoginAction(string username, string AuthType, string events)
        {
            UserName = username;
            this.AuthType = AuthType;
            Key = string.Empty;
            PassWord = string.Empty;
            Events = events;

        }
        public LoginAction(string username, string AuthType, string key, string events)
        {
            UserName = username;
            this.AuthType = AuthType;
            Key = key;
            Events = events;
            PassWord = string.Empty;

        }

    }
}
