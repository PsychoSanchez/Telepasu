using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re_translator.ServerEntities
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
        AsteriskAction,
        AsteriskEvent,
        AsteriskResponse,
        ChatMessage,
        SqlMessage,
        InnerMessage
    }
}
