using FluentNHibernate.Mapping;

namespace DataBaseTest
{
    class ContactsBookMap : ClassMap<ContactsBook>
    {
        public ContactsBookMap()
        {
            Id(c => c.ID);
            Map(c => c.ServerUserID);
            Map(c => c.UserID);
        }
    }
}
