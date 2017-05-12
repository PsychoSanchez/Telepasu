using FluentNHibernate.Mapping;

namespace DataBaseTest
{
    class UserInfoMap : ClassMap<UserInfo>
    {
        public UserInfoMap()
        {
            Id(c => c.ID);
            Map(c => c.Name);
            Map(c => c.LastName);
            Map(c => c.FirstName);
        }
    }
}
