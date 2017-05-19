using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.LocalDB.SubsTable
{
    class SubscribtionMapper : ClassMap<Subscribtions>
    {
        public SubscribtionMapper()
        {
            Id(c => c.ID);
            Map(c => c.APP_ID);
            Map(c => c.MESSAGE_TAG);
            Map(c => c.USER_ID);
        }
    }
}
