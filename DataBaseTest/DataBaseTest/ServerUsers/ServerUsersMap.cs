using FluentNHibernate.Mapping;

namespace DataBaseTest
{
    class ServerUsersMap : ClassMap<ServerUsers>
    {
        public ServerUsersMap()
        {
            Id(c => c.ID);
            Map(c => c.Name);
            Map(c => c.LastName);
            Map(c => c.FirstName);
            Map(c => c.StatusID);
            Map(c => c.NumberID);
            Map(c => c.Password);
            Map(c => c.RightsID);
        }
    }
}
