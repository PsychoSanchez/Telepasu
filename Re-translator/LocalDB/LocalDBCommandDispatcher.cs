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
using FluentNHibernate.Visitors;
using Proxy.Helpers;

namespace Proxy.LocalDB
{
    public class LocalDbResponse
    {
        public object Data;
        public int Status;
        public LocalDbResponse(object data, int status)
        {
            Data = data;
            Status = status;
        }
    }
    public class LocalDbCommandDispatcher
    {
        private static ISessionFactory _sf;

        #region InitDBMethods
        public bool ConnectLocalDb()
        {
            Configuration cfg = new Configuration();
            try
            {
                _sf = cfg.Configure().BuildSessionFactory();
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
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<WhiteList.WhiteList>())
                .BuildConfiguration();
            var exporter = new SchemaUpdate(createDBcofig);
            exporter.Execute(true, true);

            _sf = createDBcofig.Configure().BuildSessionFactory();
        }
        #endregion

        #region AddMethods
        public LocalDbResponse AddAppUid(int uid)
        {
            try
            {
                if (CheckAppUid(uid) != null) return new LocalDbResponse(false, 401);

                _sf.OpenSession().Save(new Applications()
                {
                    APP_ID = uid
                });

                return new LocalDbResponse(true, 200);
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return new LocalDbResponse(false, 408);
            }
        }

        public bool AddWhiteListItem(string address)
        {
            try
            {
                var whiteListRow =
                    _sf.OpenSession()
                        .QueryOver<WhiteList.WhiteList>()
                        .Where(x => x.Address == address)
                        .SingleOrDefault<WhiteList.WhiteList>();

                if (whiteListRow == null)
                {
                    _sf.OpenSession().Save(new WhiteList.WhiteList
                    {
                        Address = address
                    });
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

        public LocalDbResponse AddSubscribtion(string messageTag, int appId, int userId)
        {
            try
            {
                if (GetSubscribtions(messageTag, appId, userId) != null) return new LocalDbResponse(false, 401);

                Subscribtions sub = new Subscribtions
                {
                    APP_ID = appId,
                    MESSAGE_TAG = messageTag,
                    USER_ID = userId
                };
                _sf.OpenSession().Save(sub);

                return new LocalDbResponse(true, 200);
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return new LocalDbResponse(false, 408);
            }
        }

        public LocalDbResponse AddUser(string login, string secret, string role)
        {
            try
            {
                if (GetUser(login, secret, null).Data != null) return new LocalDbResponse(false, 401);

                Users user = new Users
                {
                    Login = login,
                    Password = secret,
                    Role = role
                };

                _sf.OpenSession().Save(user);

                return new LocalDbResponse(true, 200);
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return new LocalDbResponse(false, 408);
            }
        }
        #endregion

        #region UpdateMethods

        public LocalDbResponse UpdateUserPassword(string login, string newPassword)
        {
            try
            {
                var response = GetUser(login);
                var user = response.Data as Users;
                if (user == null) return new LocalDbResponse(null, 404);
                
                var query = "update Users set Password = '"+newPassword+"' where Login = :login";
                var update = _sf.OpenSession().CreateQuery(query)
                                    .SetParameter("login", login);
                update.ExecuteUpdate();
                return new LocalDbResponse(null, 200);
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return new LocalDbResponse(null, 408);
            }
        }

        #endregion

        #region GetMethods

        public List<WhiteList.WhiteList> GetWhiteList()
        {
            try
            {
                return _sf.OpenSession().CreateCriteria<WhiteList.WhiteList>().List<WhiteList.WhiteList>() as List<WhiteList.WhiteList>;
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return null;
            }
        }

        public bool CheckWhiteList(string address)
        {
            try
            {
                var whiteListRow =
                    _sf.OpenSession()
                        .QueryOver<WhiteList.WhiteList>()
                        .Where(x => x.Address == address)
                        .SingleOrDefault<WhiteList.WhiteList>();
                return (whiteListRow != null);
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return false;
            }
        }
        public LocalDbResponse CheckAppUid(int uidToFind)
        {
            try
            {
                var asd = _sf.OpenSession().CreateCriteria<Applications>().List<Applications>();

                foreach (var item in asd)
                {
                    if (item.APP_ID == uidToFind)
                        return new LocalDbResponse(item, 200);
                }

                return new LocalDbResponse(null, 404);
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return new LocalDbResponse(null, 408);
            }
        }

        public LocalDbResponse GetUser(string login)
        {
            try
            {
                var asd = _sf.OpenSession().CreateCriteria<Users>().List<Users>();

                foreach (Users item in asd)
                {
                    if (item.Login == login)
                    {
                        return new LocalDbResponse(item, 200);
                    }
                }

                return new LocalDbResponse(null, 404);
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return new LocalDbResponse(null, 408);
            }
        }

        public List<Users> GetAllUsers()
        {
            try
            {
                return _sf.OpenSession().CreateCriteria<Users>().List<Users>() as List<Users>;
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return null;
            }
        }

        public LocalDbResponse GetUser(string login, string secret, string actionChallenge)
        {
            try
            {
                var asd = _sf.OpenSession().CreateCriteria<Users>().List<Users>();

                foreach (var item in asd)
                {
                    if (actionChallenge == null)
                    {
                        if ((item.Login == login) && (item.Password == secret))
                        {
                            return new LocalDbResponse(item, 200);
                        }
                    }
                    if (item.Login != login) continue;
                    var stringToHash = item.Password + actionChallenge;
                    var hash = Encryptor.CalculateMD5Hash(stringToHash);
                    if (hash == secret) return new LocalDbResponse(item, 200);
                }
                return new LocalDbResponse(null, 404);

            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return new LocalDbResponse(null, 408);
            }
        }
        public LocalDbResponse GetSubscribtions(int appId)
        {
            try
            {
                var list = _sf.OpenSession().CreateCriteria<Subscribtions>().List<Subscribtions>();
                List<Subscribtions> temp = new List<Subscribtions>();

                foreach (var item in list)
                {
                    if (item.APP_ID == appId)
                        temp.Add(item);
                }

                return new LocalDbResponse(temp, 200);
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return new LocalDbResponse(null, 408);
            }
        }

        public LocalDbResponse GetSubscribtions(string messageTag, int appId, int userId)
        {
            try
            {
                var list = _sf.OpenSession().CreateCriteria<Subscribtions>().List<Subscribtions>();
                List<Subscribtions> temp = new List<Subscribtions>();

                foreach (var item in list)
                {
                    if ((item.MESSAGE_TAG == messageTag) && (item.APP_ID == appId) && (item.USER_ID == userId))
                        temp.Add(item);
                }

                return new LocalDbResponse(temp, 200);
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return new LocalDbResponse(null, 408);
            }
        }

        public LocalDbResponse GetSubscribtions(string messageTag)
        {
            try
            {
                var list = _sf.OpenSession().CreateCriteria<Subscribtions>().List<Subscribtions>();
                List<Subscribtions> temp = new List<Subscribtions>();

                foreach (var item in list)
                {
                    if (item.MESSAGE_TAG == messageTag)
                        temp.Add(item);
                }

                return new LocalDbResponse(temp, 200);
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
                return new LocalDbResponse(null, 408);
            }
        }
        #endregion

        #region DeleteMethods

        public void DeleteWhiteList(string address)
        {
            try
            {
                var session = _sf.OpenSession();
                session.BeginTransaction();
                session.CreateSQLQuery("delete WhiteList where Address = " + address);
                session.Close();
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
            }

        }
        public void DeleteSub(string messageTag, int appId)
        {
            try
            {
                var session = _sf.OpenSession();
                session.BeginTransaction();
                session.CreateSQLQuery("delete Subsctibtions where APP_ID = " + appId + " and MESSAGE_TAG = " + messageTag);
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
                var session = _sf.OpenSession();
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
            _sf.Dispose();
        }

        public void DropTable(string tableName)
        {
            try
            {
                _sf.OpenSession().CreateQuery("delete " + tableName + " e").ExecuteUpdate();
                var asd = _sf.OpenSession().CreateCriteria<Users>().List<Users>();
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
            }
        }
        #endregion

    }
}
