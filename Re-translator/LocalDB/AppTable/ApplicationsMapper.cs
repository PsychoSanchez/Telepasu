using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.LocalDB.AppTable
{
    public class ApplicationsMapper : ClassMap<Applications>
    {
        public ApplicationsMapper()
        {
            Id(c => c.ID);
            Map(c => c.APP_ID);
        }
    }
}
