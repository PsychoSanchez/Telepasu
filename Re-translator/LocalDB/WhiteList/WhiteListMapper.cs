using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Proxy.LocalDB.WhiteList
{
    public class WhiteListMapper : ClassMap<WhiteList>
    {
        public WhiteListMapper()
        {
            Id(x => x.ID);
            Map(x => x.Address);
        }
    }
}
