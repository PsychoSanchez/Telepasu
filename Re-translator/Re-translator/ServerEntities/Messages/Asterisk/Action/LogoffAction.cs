namespace Proxy.ServerEntities.Messages
{
    class LogoffAction : AsteriskAction
    {
        /// <summary>
        /// Возвращает название действия
        /// </summary>
        public override string Action
        {
            get
            {
                return "Logoff";
            }
        }

        //public override string Parameters
        //{
        //    get
        //    {
        //        return "";
        //    }
        //}
    }
}
