using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Proxy.LocalDB.AppTable;
using Proxy.LocalDB.SubsTable;
using Proxy.LocalDB.UsersTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.LocalDB
{
    public class LocalDBCommandDispatcher
    {
        private TcpClient _client;
        private static ISessionFactory sf;

        public LocalDBCommandDispatcher()
        {

        }

        public bool ConnectLocalDB()
        {
            Configuration cfg = new Configuration();
            try
            {
                sf = cfg.Configure().BuildSessionFactory();
            }
            catch (SocketException)
            {
                Console.WriteLine("Unable to connect to database");
                return false;
            }

            return true;
        }

        public void InitTables()
        {
            var createDBcofig = Fluently.Configure().Database(PostgreSQLConfiguration.Standard.ShowSql)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SubscribtionMapper>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UsersMapper>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ApplicationsMapper>())
                .BuildConfiguration();
            var exporter = new SchemaUpdate(createDBcofig);
            exporter.Execute(true, true);
        }


    }
}
