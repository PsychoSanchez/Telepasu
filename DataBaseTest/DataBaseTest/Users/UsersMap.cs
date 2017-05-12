using FluentNHibernate.Mapping;

namespace DataBaseTest
{
    class UsersMap : ClassMap<Users>
    {
        public UsersMap()
        {
            Id(c => c.ID);
            Map(c => c.NumberID);
            Map(c => c.UserInfoID);
            Map(c => c.ServerUserID);
            Map(c => c.NumberTypeID);
        }
    }
}
