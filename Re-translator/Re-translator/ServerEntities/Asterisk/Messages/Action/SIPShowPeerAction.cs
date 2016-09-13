namespace Proxy.ServerEntities.Messages
{
    class SIPShowPeerAction : AsteriskAction
    {
        public override string Action
        {
            get
            {
                return "SIPShowPeer";
            }
        }
        public string Peer { get; set; }
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public SIPShowPeerAction()
        {
            Peer = string.Empty;
        }
        /// <summary>
        /// Запрос определенного пира с сервера
        /// </summary>
        /// <param name="peer"></param>
        public SIPShowPeerAction(string peer)
        {
            this.Peer = peer;
        }

    }
}
