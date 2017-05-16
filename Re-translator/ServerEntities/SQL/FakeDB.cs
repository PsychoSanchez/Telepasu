using Proxy.Helpers;
using System;
using System.Linq;
using System.Net.Sockets;

namespace Proxy.ServerEntities.SQL
{
    class FakeDB : EntityManager, IDB
    {
        private readonly string[] _users = { "mark", "oleg", "Oleg", "Olegen'ka", "Olga" };
        private readonly string[] _password = { "1488", "1488", "1488", "1488", "1488" };
        public FakeDB()
        {
        }
        public FakeDB(TcpClient tcp, Socket client) : base(tcp, client)
        {
        }

        /*
         *  Методы для авторизации в базе данных 
         */
        public bool Authentificate(string username, string password)
        {
            if (!IsUserExist(username))
            {
                return false;
            }
            var pwd = _password[(int)FindUserByUsername(username)].ToString();
            return (pwd == password);
        }
        public bool Authentificate(string username, string pass, string md5Challenge)
        {
            if (!IsUserExist(username))
            {
                return false;
            }
            var pwd = _password[(int)FindUserByUsername(username)];
            var encryptedpwd = Encryptor.CalculateMD5Hash(md5Challenge + pwd);
            return (encryptedpwd == pass);        
        }
        public object FindUserById(int id)
        {
            throw new NotImplementedException();
        }

        public object FindUserByUsername(string username)
        {
            // Decrypt
            return Array.IndexOf(_users, username);
        }

        public bool IsUserExist(string username)
        {
            return _users.Any(user => user == username);
        }

        public bool ConnectDB(string dbname, string login, string pwd, string ip, string port)
        {
            throw new NotImplementedException();
        }

        public bool ConnectDB(string login, string pwd, string ip, string port)
        {
            return true;
        }

        protected override void ObtainMessage(object sender, MessageArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void Disconnected(object sender, MessageArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void WorkAction()
        {
            throw new NotImplementedException();
        }

        public EntityManager GetDB()
        {
            return this;
        }
    }
}
