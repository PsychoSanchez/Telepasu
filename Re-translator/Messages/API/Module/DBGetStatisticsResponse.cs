using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Data;
using Proxy.Messages.API.Admin;
using Proxy.ServerEntities;

namespace Proxy.Messages.API.Module
{
    public struct StatItem
    {
        public string number;
        public string percentage;
    }
    public class DBGetStatisticsResponse : MethodCall
    {
        public string Tag { get; set; }
        public string Action { get; set; }
        public List<StatItem> statistics = new List<StatItem>();
        public string UserNumber;

        public DBGetStatisticsResponse(EntityManager sender) : base(sender)
        {
            
        }
    }
}
