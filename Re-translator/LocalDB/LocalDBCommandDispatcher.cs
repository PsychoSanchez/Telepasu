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

        #region InitDBMethods
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

            sf = createDBcofig.Configure().BuildSessionFactory();
        }
        #endregion

        #region AddMethods
        public bool AddAppUid(int uid)
        {
            try
            {
                if (CheckAppUID(uid) != null) return false;

                sf.OpenSession().Save(new Applications()
                {
                    APP_ID = uid
                });

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
                if (GetSubscribtions(message_tag, app_id, user_id) != null) return false;

                Subscribtions sub = new Subscribtions
                {
                    APP_ID = app_id,
                    MESSAGE_TAG = message_tag,
                    USER_ID = user_id
                };
                sf.OpenSession().Save(sub);

                var list = sf.OpenSession().CreateCriteria<Subscribtions>().List<Subscribtions>();

                return true;
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return false;
            }
        }

        public bool AddUser(string login, string secret, string role)
        {
            try
            {
                if (GetUser(login, secret) != null) return false;

                Users user = new Users
                {
                    Login = login,
                    Password = secret,
                    Role = role
                };

                sf.OpenSession().Save(user);

                return true;
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return false;
            }
        }
        #endregion

        #region GetMethods
        public Applications CheckAppUID(int uid_to_find)
        {
            var asd = sf.OpenSession().CreateCriteria<Applications>().List<Applications>();

            foreach (var item in asd)
            {
                if (item.APP_ID == uid_to_find)
                    return item;
            }

            return null;
        }


        public Users GetUser(string login, string secret)
        {
            try
            {
                var asd = sf.OpenSession().CreateCriteria<Users>().List<Users>();

                foreach (Users item in asd)
                {
                    if ((item.Login == login) && (item.Password == secret))
                    {
                        return item;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Subscribtions> GetSubscribtions(int app_id)
        {
            try
            {
                var list = sf.OpenSession().CreateCriteria<Subscribtions>().List<Subscribtions>();
                List<Subscribtions> temp = new List<Subscribtions>();

                foreach (var item in list)
                {
                    if (item.APP_ID == app_id)
                        temp.Add(item);
                }

                return temp;
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return null;
            }
        }

        public List<Subscribtions> GetSubscribtions(string message_tag, int app_id, int user_id)
        {
            try
            {
                var list = sf.OpenSession().CreateCriteria<Subscribtions>().List<Subscribtions>();
                List<Subscribtions> temp = new List<Subscribtions>();

                foreach (var item in list)
                {
                    if ((item.MESSAGE_TAG == message_tag) && (item.APP_ID == app_id) && (item.USER_ID == user_id))
                        temp.Add(item);
                }

                return temp;
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
                var list = sf.OpenSession().CreateCriteria<Subscribtions>().List<Subscribtions>();
                List<Subscribtions> temp = new List<Subscribtions>();

                foreach (var item in list)
                {
                    if (item.MESSAGE_TAG == message_tag)
                        temp.Add(item);
                }

                return temp;
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return null;
            }
        }
        #endregion

        #region DeleteMethods
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
        public void Dispose()
        {
            _client.Dispose();
            sf.Dispose();
        }
        #endregion

    }
}
