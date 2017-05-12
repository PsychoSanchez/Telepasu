using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DataBaseTest
{
    static class DataBaseModule
    {
        //static Socket mainSocket = new Socket(SocketType.Rdm, ProtocolType.IP);
        public static ISessionFactory sf;
        private static ConcurrentBag<string> messageQuery = new ConcurrentBag<string>();
        static void Main(string[] args)
        {
            Configuration cfg = new Configuration();
            try
            {
                sf = cfg.Configure().BuildSessionFactory();
            }
            catch (SocketException)
            {
                Console.WriteLine("Unable to connect to database");
                return;
            }

            var createDBcofig = Fluently.Configure().Database(PostgreSQLConfiguration.Standard.ShowSql)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CallHistoryMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ContactsBookMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NoteTypesMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NumbersMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NumberTypesMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<RightsMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ServerUsersMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<StatusesMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserDataMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserInfoMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UsersMap>())
                .BuildConfiguration();
            var exporter = new SchemaUpdate(createDBcofig);
            exporter.Execute(true, true);

            sf = createDBcofig.BuildSessionFactory();

            getMessage(5433);
            handleCommandsQuery();

            Console.WriteLine("123");
            //ServerUsers user = new ServerUsers
            //{
            //    NumberID = 442,
            //    Password = "5432"
            //};

            //using (var session = sf.OpenSession())
            //{
            //    ITransaction tr = session.BeginTransaction();
            //    session.Save(user);
            //    tr.Commit();
            //}
            
            //var user_temp = sf.OpenSession().CreateCriteria<ServerUsers>().List<ServerUsers>();

            //var wtf = (from p in sf.OpenSession().Query<CallHistory>()
            //           where p.Number == "123"
            //           select p).FirstOrDefault();

            //Console.WriteLine(user_temp[0].Password);

            Console.ReadLine();
        }

        static async void getMessage(int port)
        {
            await Task.Run(() => listenPort(port));
        }

        static void listenPort(int port)
        {
            TcpListener tcplistner = new TcpListener(System.Net.IPAddress.Loopback, port);
            tcplistner.Start();

            Byte[] bytes = new Byte[256];

            TcpClient incoming = tcplistner.AcceptTcpClient();

            NetworkStream incomingStream = incoming.GetStream();

            int i = 0;
            while ((i = incomingStream.Read(bytes, 0, bytes.Length)) != 0)
            {
                string message = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                messageQuery.Add(message);
                Console.WriteLine(message);
            }
        }

        static async void handleCommandsQuery()
        {
            await Task.Run(() => handleCommand());
        }

        static void handleCommand()
        {
            string error = "";
            while (true)
            {
                string command;
                messageQuery.TryTake(out command);

                if (command != null)
                {
                    CommandTemplate parsedCommand = JsonConvert.DeserializeObject<CommandTemplate>(command);

                    switch (parsedCommand.CommandTask)
                    {
                        case "get_contacts":
                            {
                                Console.WriteLine(parsedCommand.CommandData);
                                break;
                            }
                        default:
                            {
                                Console.WriteLine(error = "unknown command");
                                break;
                            }
                    }
                }
            }
        }
    }
}
