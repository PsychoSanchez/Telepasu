using Proxy.ServerEntities;

namespace Proxy.Messages.API.Admin
{
    internal class RemoveWhiteListMethod: MethodCall
    {
        public string Address;

        public RemoveWhiteListMethod(EntityManager sender) : base(sender)
        {
            Action = "Remove White List";
        }
    }
}
