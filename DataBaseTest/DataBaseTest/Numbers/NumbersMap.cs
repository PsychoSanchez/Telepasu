using FluentNHibernate.Mapping;

namespace DataBaseTest
{
    class NumbersMap : ClassMap<Numbers>
    {
        public NumbersMap()
        {
            Id(c => c.ID);
            Map(c => c.Number);
            Map(c => c.StatusID);
        }
    }
}
