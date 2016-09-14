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
        public string AuthType { get; }
        public ChallengeAction()
        {
            AuthType = "MD5";
        }

    }
}
