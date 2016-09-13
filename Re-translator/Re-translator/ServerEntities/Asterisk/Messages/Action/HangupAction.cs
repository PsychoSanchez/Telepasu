namespace Proxy.ServerEntities.Messages
{
    class HangupAction : AsteriskAction
    {
        public override string Action
        {
            get
            {
                return "Hangup";
            }
        }
        public string Channel { get; set; }
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public HangupAction()
        {
        }
        /// <summary>
        /// Создает Hangup Action с названием канала, который нужно закрыть
        /// </summary>
        /// <param name="channel"></param>
        public HangupAction(string channel)
        {
            Channel = channel;
        }

    }
}
