using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Proxy.LocalDB.AppTable;
using Proxy.LocalDB.SubsTable;
using Proxy.LocalDB.UsersTable;
using System;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Proxy.LocalDB
{
    public class LocalDBCommandDispatcher
    {
        private TcpClient _client;
        private static ISessionFactory sf;

        public LocalDBCommandDispatcher()
        {

        }

        public void Dispose()
        {
            _client.Dispose();
            sf.Dispose();
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

        public Applications CheckAppUID(int uid_to_find)
        {
            Applications temp = (from p in sf.OpenSession().QueryOver<Applications>() where p.APP_ID == uid_to_find select p).SingleOrDefault<Applications>();
            return temp;
        }

        public bool AddAppUid(int uid)
        {
            try
            {
                Applications temp = (from p in sf.OpenSession().QueryOver<Applications>() where p.APP_ID == uid select p).SingleOrDefault<Applications>();
                if (temp == null)
                {
                    var session = sf.OpenSession();
                    session.BeginTransaction();
                    session.Save(new Applications()
                    {
                        APP_ID = uid
                    });
                    session.Close();
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return false;
            }
        }

        public void DeleteAppUid(int uid)
        {
            try
            {
                var session = sf.OpenSession();
                Applications app = new Applications
                {
                    APP_ID = uid
                };
                session.BeginTransaction();
                session.Delete(app);
                session.Close();
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
            }
        }

        public Users GetUser(string login, string secret)
        {
            try
            {
                Users temp = (from p in sf.OpenSession().QueryOver<Users>() where (p.Login == login && p.Password == secret) select p).SingleOrDefault<Users>();
                return temp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool AddUser(string login, string secret)
        {
            try
            {
                Users user = new Users
                {
                    Login = login,
                    Password = secret
                };
                var session = sf.OpenSession();
                session.BeginTransaction();
                session.Save(user);
                session.Close();
                return true;
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return false;
            }
        }

        public bool AddSubscribtion(string message_tag, int app_id, int user_id)
        {
            try
            {
                Subscribtions sub = new Subscribtions
                {
                    APP_ID = app_id,
                    MESSAGE_TAG = message_tag,
                    USER_ID = user_id
                };
                var session = sf.OpenSession();
                session.BeginTransaction();
                session.Save(sub);
                session.Close();
                return true;
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return false;
            }
        }

        public List<Subscribtions> GetSubscribtions(int app_id)
        {
            try
            {
                List<Subscribtions> list = (List<Subscribtions>)(from p in sf.OpenSession().QueryOver<Subscribtions>() where p.APP_ID == app_id select p).List<Subscribtions>();
                return list;
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return null;
            }
        }
        public List<Subscribtions> GetSubscribtions(string message_tag)
        {
            try
            {
                List<Subscribtions> list = (List<Subscribtions>)(from p in sf.OpenSession().QueryOver<Subscribtions>() where p.MESSAGE_TAG == message_tag select p).List<Subscribtions>();
                return list;
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return null;
            }
        }

        public void DeleteSub(string message_tag, int app_id)
        {
            try
            {
                var session = sf.OpenSession();
                session.BeginTransaction();
                session.CreateSQLQuery("delete Subsctibtions where APP_ID = " + app_id.ToString() + " and MESSAGE_TAG = " + message_tag);
                session.Close();
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
            }
        }
    }
}
