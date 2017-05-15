using System;
using Proxy.Helpers;
using Proxy.ServerEntities;
using Proxy.ServerEntities.SQL;
using Proxy.ServerEntities.Users;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Proxy
{
    public class MailPost
    {
        /// <summary>
        /// Key - Tag (string)
        /// </summary>
        private readonly ConcurrentDictionary<string, ConcurrentList> _subscribers = new ConcurrentDictionary<string, ConcurrentList>();
        private readonly ConcurrentList moduleList = new ConcurrentList();
        private readonly ConcurrentList appsList = new ConcurrentList();
        //public AsteriskEntity Asterisk;
        //private UserManager _database;
        //public IDB DB;

        public string AsteriskVersion { get; set; } = "1.1";


        public void AddApplication(string uid, UserManager user)
        {
            
        }
        public void AddNativeModule(string uid, UserManager user)
        {
            // TODO: Change user to nativemodule class
        }

        public void AddModule(string uid, UserManager user)
        {
            
        }

        public void Subscribe(UserManager user, string tag)
        {

        }

        public void Unsubscrive(UserManager user, string tag)
        {
            
        }
        public void PostMessage(ServerMessage message)
        {
            // TODO: Semaphore synchronization
            if (!_subscribers.ContainsKey(message.Tag))
            {
                return;
            }
            var subscribers = _subscribers[message.Tag];
            foreach (var subscriber in subscribers)
            {
                subscriber.SendMesage(message);
            }
        }
        //public void AddUser(UserManager user)
        //{
        //    if (_mailbox.ContainsKey(user))
        //    {
        //        return;
        //    }
        //    ConcurrentQueue<ServerMessage> messages = new ConcurrentQueue<ServerMessage>();
        //    _mailbox.AddOrUpdate(user, messages, (key, oldValue) => messages);
        //    _usersList.Add(user);
        //}
        //public void AddAsterisk(AsteriskEntity asterisk)
        //{
        //    Asterisk = asterisk;
        //    ConcurrentQueue<ServerMessage> messages = new ConcurrentQueue<ServerMessage>();
        //    _mailbox.AddOrUpdate(asterisk, messages, (key, oldValue) => messages);
        //}
        //public void AddDb(IDB db)
        //{
        //    DB = db;
        //    _database = db.GetDB();
        //    ConcurrentQueue<ServerMessage> messages = new ConcurrentQueue<ServerMessage>();
        //    _mailbox.AddOrUpdate(_database, messages, (key, oldValue) => messages);
        //}

        //public List<UserManager> GetUsers()
        //{
        //    return _usersList.ToList();
        //}
        //public void DeleteUser(UserManager user)
        //{
        //    _usersList.Remove(user);
        //    ConcurrentQueue<ServerMessage> ignored;
        //    _mailbox.TryRemove(user, out ignored);
        //}
        //public void Clear()
        //{
        //    // Дописать лок аглок для списков и удалить ключи с массивами из словаря
        //    _usersList.Clear();
        //}



        //public List<ServerMessage> GrabMessages(UserManager entity)
        //{
        //    if (!_mailbox.ContainsKey(entity))
        //    {
        //        // TODO: ДОПИЛИТЬ ЭКЗЕБЖОН
        //        return null;
        //    }
        //    List<ServerMessage> temp = new List<ServerMessage>();
        //    while (!_mailbox[entity].IsEmpty)
        //    {
        //        ServerMessage item;
        //        if (_mailbox[entity].TryDequeue(out item))
        //        {
        //            temp.Add(item);
        //        }
        //    }
        //    return temp;
        //}
    }
}
