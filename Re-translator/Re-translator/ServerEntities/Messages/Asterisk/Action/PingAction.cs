namespace Proxy.ServerEntities.Messages
{
    class PingAction : AsteriskAction
    {
        public override string Action
        {
            get
            {
                return "Ping";
            }
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
