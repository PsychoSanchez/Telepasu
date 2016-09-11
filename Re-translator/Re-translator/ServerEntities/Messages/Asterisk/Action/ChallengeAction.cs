namespace Proxy.ServerEntities.Messages
{
    class ChallengeAction : AsteriskAction
    {
        public string AuthType;
        public ChallengeAction()
        {
            AuthType = "MD5";
        }
        public override string Action
        {
            get
            {
                return "Challenge";
            }
        }
    }
}
