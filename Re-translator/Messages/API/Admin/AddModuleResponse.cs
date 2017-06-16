using Newtonsoft.Json;
using Proxy.ServerEntities;

namespace Proxy.Messages.API.Admin
{
    internal class AddModuleResponse
    {
        public string Action = "Add Module";
        public string Type = "Module";
        public bool Connected = true;
        public string Status = "200";
    }
}
