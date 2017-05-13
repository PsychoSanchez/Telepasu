using FluentNHibernate.Mapping;

namespace DataBaseTest
{
    class NumberTypesMap : ClassMap<NumberTypes>
    {
        public NumberTypesMap()
        {
            Id(c => c.ID);
            Map(c => c.NumberType);
        }
    }
}
