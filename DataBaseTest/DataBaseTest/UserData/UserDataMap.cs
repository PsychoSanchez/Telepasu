using FluentNHibernate.Mapping;

namespace DataBaseTest
{
    class UserDataMap : ClassMap<UserData>
    {
        public UserDataMap()
        {
            Id(c => c.ID);
            Map(c => c.NoteTypeID);
            Map(c => c.UserInfoID);
            Map(c => c.Value);
        }
    }
}
