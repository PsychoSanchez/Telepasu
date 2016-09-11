namespace Proxy.ServerEntities.Messages
{
    class ParkedCallsAction : AsteriskAction
    {
        public ParkedCallsAction()
        {

        }
        public override string Action
        {
            get
            {
                return "ParkedCalls";
            }
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
