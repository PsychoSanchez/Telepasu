using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.LocalDB.UsersTable
{
    public class Users
    {
        public virtual int ID { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual string Role { get; set; }
    }
}
