using System;
using Proxy.Helpers;
using Proxy.ServerEntities;
using Proxy.ServerEntities.SQL;
using Proxy.ServerEntities.Users;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Proxy
{
    public class DeMail
    {
        private readonly ConcurrentDictionary<UserManager, ConcurrentQueue<ServerMessage>> _mailbox = new ConcurrentDictionary<UserManager, ConcurrentQueue<ServerMessage>>();
        private readonly ConcurrentList _usersList = new ConcurrentList();
        public AsteriskEntity Asterisk;
        private UserManager _database;
        public IDB DB;

        public string AsteriskVersion { get; set; } = "1.1";

        public void AddUser(UserManager user)
        {
            if (_mailbox.ContainsKey(user))
            {
                return;
            }
            ConcurrentQueue<ServerMessage> messages = new ConcurrentQueue<ServerMessage>();
            _mailbox.AddOrUpdate(user, messages, (key, oldValue) => messages);
            _usersList.Add(user);
        }
        public void AddAsterisk(AsteriskEntity asterisk)
        {
            Asterisk = asterisk;
            ConcurrentQueue<ServerMessage> messages = new ConcurrentQueue<ServerMessage>();
            _mailbox.AddOrUpdate(asterisk, messages, (key, oldValue) => messages);
        }
        public void AddDb(IDB db)
        {
            DB = db;
            _database = db.GetDB();
            ConcurrentQueue<ServerMessage> messages = new ConcurrentQueue<ServerMessage>();
            _mailbox.AddOrUpdate(_database, messages, (key, oldValue) => messages);
        }

        public List<UserManager> GetUsers()
        {
            return _usersList.ToList();
        }
        public void DeleteUser(UserManager user)
        {
            _usersList.Remove(user);
            ConcurrentQueue<ServerMessage> ignored;
            _mailbox.TryRemove(user, out ignored);
        }
        public void Clear()
        {
            // Дописать лок аглок для списков и удалить ключи с массивами из словаря
            _usersList.Clear();
        }


        public void PostMessage(ServerMessage message)
        {
            switch (message.type)
            {
                case MessageType.AsteriskAction:
                    if (Asterisk != null)
                    {
                        _mailbox[Asterisk].Enqueue(message);
                    }
                    break;
                case MessageType.AsteriskMessage:
                    foreach (var user in _usersList)
                    {
                        _mailbox[user].Enqueue(message);
                    }
                    break;
                case MessageType.ChatMessage:
                    // Запись в бд
                    // Отправка клиентам
                    break;
                case MessageType.InnerMessage:
                    //????Какая то хуйня
                    break;
                case MessageType.SqlMessage:
                    // Обращение к бд
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public List<ServerMessage> GrabMessages(UserManager entity)
        {
            if (!_mailbox.ContainsKey(entity))
            {
                // ДОПИЛИТЬ ЭКЗЕБЖОН
                return null;
            }
            List<ServerMessage> temp = new List<ServerMessage>();
            while (!_mailbox[entity].IsEmpty)
            {
                ServerMessage item;
                if (_mailbox[entity].TryDequeue(out item))
                {
                    temp.Add(item);
                }
            }
            return temp;
        }
    }
}
