using FluentNHibernate.Mapping;

namespace DataBaseTest
{
    class StatusesMap : ClassMap<Statuses>
    {
        public StatusesMap()
        {
            Id(c => c.ID);
            Map(c => c.StatusName);
        }
    }
}
