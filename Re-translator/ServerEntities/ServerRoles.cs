namespace Proxy.ServerEntities
{
    public enum UserRole {
        Admin,
        HardUser,
        User,
        SuperUser,
        Manager,
        Guest
    }
    public enum MessageType
    {
        AsteriskMessage, // Сообщение от сервера, отправить всем клиентам
        AsteriskAction, //Сообщение для отправки на сервер
        ChatMessage, // Сообщение от чата, доставить конкретным пользователям
        SqlMessage,
        InnerMessage // Сообщение от админпанели для управления.
    }
    public enum AsteriskMessageType
    {
        Event,
        Response,
        Action,
        Default
    }
}
