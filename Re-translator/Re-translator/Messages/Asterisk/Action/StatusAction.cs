namespace Proxy.ServerEntities.Messages
{
    class StatusAction : AsteriskAction
    {
        public override string Action
        {
            get
            {
                return "Status";
            }
        }
        public string Channel { get; set; }
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public StatusAction()
        {
        }
        /// <summary>
        /// Создает Hangup Action с названием канала, который нужно закрыть
        /// </summary>
        /// <param name="channel"></param>
        public StatusAction(string channel)
        {
            Channel = channel;
        }
    }
}
