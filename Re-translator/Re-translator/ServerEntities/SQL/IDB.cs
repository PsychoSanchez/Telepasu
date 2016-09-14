using System.Collections.Generic;

namespace Proxy.ServerEntities.SQL
{
    public interface IDB
    {
        bool ConnectDB(string dbname, string login, string pwd, string ip, string port);
        bool ConnectDB(string login, string pwd, string ip, string port);
        UserManager GetDB();
        //bool ConnectDB(string dbname, string login, string pwd, string domain);
        bool IsUserExist(string username);
        object FindUserByUsername(string _username);
        object FindUserById(int _id);
        bool Authentificate(string username, string password);
        bool Authentificate(string username, string password, string MD5Challenge);
        //object GetMessageById(string id);
        //List<object> GetUnreadMessages(string username);
    }
}
