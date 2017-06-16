using Proxy.ServerEntities;

namespace Proxy.Messages.API.Admin
{
    internal class AddWhiteListMethod: MethodCall
    {
        public string Address;

        public AddWhiteListMethod(EntityManager sender) : base(sender)
        {
            Action = "Add White List";
        }
    }
}
