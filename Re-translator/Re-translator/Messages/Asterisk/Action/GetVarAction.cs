namespace Proxy.ServerEntities.Messages
{
    class GetVarAction : AsteriskAction
    {
        public override string Action
        {
            get
            {
                return "GetVar";
            }
        }
        public string Variable { get; set; }
        public string Channel { get; set; }
        /// <summary>
        /// Пустой конструктор по умолчанию
        /// </summary>
        public GetVarAction()
        {

        }
        /// <summary>
        /// Конаструктор для передачи серверу глобальной переменной
        /// </summary>
        /// <param name="variable"></param>
        public GetVarAction(string variable)
        {
            Variable = variable;
        }
        /// <summary>
        /// Конструктор для передачи серверу переменной в локальном канале
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="channel"></param>
        public GetVarAction(string variable, string channel)
        {
            Variable = variable;
            Channel = channel;
        }

    }
}
