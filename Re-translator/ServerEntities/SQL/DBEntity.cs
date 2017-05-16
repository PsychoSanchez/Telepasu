using Proxy.ServerEntities.SQL;
using System;
using System.Net.Sockets;

namespace Proxy.ServerEntities.Application
{
    public class DBEntity : EntityManager, IDB
    {
        public DBEntity(TcpClient tcp, Socket client) : base(tcp, client)
        {
        }

        public bool Authentificate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool Authentificate(string username, string password, string MD5Challenge)
        {
            throw new NotImplementedException();
        }

        public bool ConnectDB(string login, string pwd, string ip, string port)
        {
            throw new NotImplementedException();
        }

        public bool ConnectDB(string dbname, string login, string pwd, string ip, string port)
        {
            throw new NotImplementedException();
        }

        public object FindUserById(int _id)
        {
            throw new NotImplementedException();
        }

        public object FindUserByUsername(string _username)
        {
            throw new NotImplementedException();
        }

        public EntityManager GetDB()
        {
            throw new NotImplementedException();
        }

        public bool IsUserExist(string username)
        {
            throw new NotImplementedException();
        }

        protected override void Disconnected(object sender, MessageArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void WorkAction()
        {
            throw new NotImplementedException();
        }
    }
}
