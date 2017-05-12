using FluentNHibernate.Mapping;

namespace DataBaseTest
{
    class NoteTypesMap : ClassMap<NoteTypes>
    {
        public NoteTypesMap()
        {
            Id(c => c.ID);
            Map(c => c.Name);
        }
    }
}
