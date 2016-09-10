using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.ServerEntities
{
    public enum UserRole {
        Admin,
        User,
        SuperUser,
        Manager,
        Guest
    }
    public enum MessageType
    {
        AsteriskMessage, // Сообщение от сервера, отправить всем клиентам
        ChatMessage, // Сообщение от чата, доставить конкретным пользователям
        SqlMessage,
        InnerMessage // Сообщение от админпанели для управления.
    }
}
