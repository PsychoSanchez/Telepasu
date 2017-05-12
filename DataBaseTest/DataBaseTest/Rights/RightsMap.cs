using FluentNHibernate.Mapping;

namespace DataBaseTest
{
    class RightsMap : ClassMap<Rights>
    {
        public RightsMap()
        {
            Id(c => c.ID);
            Map(c => c.Type);
        }
    }
}
