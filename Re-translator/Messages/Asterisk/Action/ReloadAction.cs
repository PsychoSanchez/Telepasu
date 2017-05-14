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
    }
}
