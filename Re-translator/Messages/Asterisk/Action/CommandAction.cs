namespace Proxy.ServerEntities.Messages
{
    class CommandAction : AsteriskAction
    {
        public CommandAction(string command)
        {
            Command = command;
        }
        public override string Action
        {
            get
            {
                return "Command";
            }
        }
        public string Command { get; set; }

    }
}
