using System;
using Proxy.Helpers;
using Proxy.ServerEntities;
using Proxy.ServerEntities.SQL;
using Proxy.ServerEntities.Application;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Proxy
{
    public class MailPost
    {
        private const string ModuleName = "#(Subscribers Mail Post) ";
        /// <summary>
        /// Key - Tag (string)
        /// </summary>
        private readonly ConcurrentDictionary<string, ConcurrentList> _subscribers = new ConcurrentDictionary<string, ConcurrentList>();
        private readonly ConcurrentList _moduleList = new ConcurrentList();
        private readonly ConcurrentList _appsList = new ConcurrentList();
        private readonly ConcurrentList _innerModules = new ConcurrentList();
        //public AsteriskEntity Asterisk;
        //private EntityManager _database;
        //public IDB DB;

        public string AsteriskVersion { get; set; } = "1.1";


        public void AddApplication(string uid, EntityManager entity)
        {
            var subs = _subscribers.GetOrAdd(uid, new ConcurrentList());
            subs.Add(entity);
            _appsList.Add(entity);
        }
        public void AddNativeModule(string uid, EntityManager entity)
        {
            var subs = _subscribers.GetOrAdd(uid, new ConcurrentList());
            subs.Add(entity);
            _innerModules.Add(entity);
            // TODO: Change entity to nativemodule class
        }

        public void AddModule(string uid, EntityManager entity)
        {
            var subs = _subscribers.GetOrAdd(uid, new ConcurrentList());
            subs.Add(entity);
            _moduleList.Add(entity);
        }

        public void Subscribe(EntityManager entity, string tag)
        {
            var subs = _subscribers.GetOrAdd(tag, new ConcurrentList());
            subs.Add(entity);
        }

        public void Unsubscrive(EntityManager entity, string tag)
        {
            var subs = _subscribers.GetOrAdd(tag, new ConcurrentList());
            subs.Remove(entity);
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
        //public void AddUser(EntityManager entity)
        //{
        //    if (_mailbox.ContainsKey(entity))
        //    {
        //        return;
        //    }
        //    ConcurrentQueue<ServerMessage> messages = new ConcurrentQueue<ServerMessage>();
        //    _mailbox.AddOrUpdate(entity, messages, (key, oldValue) => messages);
        //    _usersList.Add(entity);
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

        //public List<EntityManager> GetUsers()
        //{
        //    return _usersList.ToList();
        //}
        //public void DeleteUser(EntityManager entity)
        //{
        //    _usersList.Remove(entity);
        //    ConcurrentQueue<ServerMessage> ignored;
        //    _mailbox.TryRemove(entity, out ignored);
        //}
        //public void Clear()
        //{
        //    // Дописать лок аглок для списков и удалить ключи с массивами из словаря
        //    _usersList.Clear();
        //}



        //public List<ServerMessage> GrabMessages(EntityManager entity)
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
