using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.LocalDB.UsersTable
{
    public class UsersMapper : ClassMap<Users>
    {
        public UsersMapper()
        {
            Id(c => c.ID);
            Map(c => c.Login);
            Map(c => c.Password);
        }
    }
}
