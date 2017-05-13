using FluentNHibernate.Mapping;

namespace DataBaseTest
{
    class CallHistoryMap : ClassMap<CallHistory>
    {
        public CallHistoryMap()
        {
            Id(c => c.ID);
            Map(c => c.Number);
            Map(c => c.Type);
        }
    }
}
