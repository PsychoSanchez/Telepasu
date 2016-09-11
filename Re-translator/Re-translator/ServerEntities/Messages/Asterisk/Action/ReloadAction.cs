using Proxy.Helpers;

namespace Proxy.ServerEntities.Messages
{
    class ReloadAction : AsteriskAction
    {
        public override string Action
        {
            get
            {
                return "Reload";
            }
        }
        public string Module { get; set; }
        public ReloadAction()
        {

        }
        public ReloadAction(string Module)
        {
            this.Module = Module;
        }
        public override string ToString()
        {
                if (!string.IsNullOrEmpty(Module))
                    return Module + Helper.LINE_SEPARATOR;
                return string.Empty;
        }
    }
}
