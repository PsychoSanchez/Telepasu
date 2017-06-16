using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxy.LocalDB.UsersTable;

namespace Proxy.Messages.API.Admin
{
    public class GetAllUsersMessage
    {
        public string Action = "Get All Users";
        public List<Users> Users = new List<Users>();

        public GetAllUsersMessage(List<Users> usr)
        {
            foreach (var VARIABLE in usr)
            {
                Users.Add(VARIABLE);
            }
        }
    }
}
