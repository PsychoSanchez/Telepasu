using Proxy.Helpers;
using System;
using System.Net.Sockets;

namespace Proxy.ServerEntities.SQL
{
    class FakeDB : UserManager, IDB
    {
        private string[] users = { "mark", "oleg", "Oleg", "Olegen'ka", "Olga" };
        private string[] password = { "1488", "1488", "1488", "1488", "1488" };
        public FakeDB()
        {
        }
        public FakeDB(Socket _client) : base(_client)
        {
        }

        public bool Authentificate(string username, string password)
        {
            if (!IsUserExist(username))
            {
                return false;
            }
            string pwd = password[(int)FindUserByUsername(username)].ToString();
            return (pwd == password);
        }
        public bool Authentificate(string username, string pass, string MD5Challenge)
        {
            if (!IsUserExist(username))
            {
                return false;
            }
            var pwd = password[(int)FindUserByUsername(username)];
            var encryptedpwd = Encryptor.CalculateMD5Hash(MD5Challenge + pwd);
            return (encryptedpwd == pass);        
        }
        public object FindUserById(int _id)
        {
            throw new NotImplementedException();
        }

        public object FindUserByUsername(string username)
        {
            ///Decrypt
            return Array.IndexOf(users, username);
        }

        public bool IsUserExist(string username)
        {
            foreach (var user in users)
            {
                if (user == username)
                {
                    return true;
                }
            }
            return false;
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

        protected override void WorkCycle()
        {
            throw new NotImplementedException();
        }

        public UserManager GetDB()
        {
            return this;
        }
    }
}
