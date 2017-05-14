namespace Proxy.ServerEntities.Messages
{
    class ChallengeAction : AsteriskAction
    {
        public override string Action
        {
            get
            {
                return "Challenge";
            }
        }
        public string AuthType { get; private set; }
        public ChallengeAction()
        {
            AuthType = "MD5";
        }

    }
}
