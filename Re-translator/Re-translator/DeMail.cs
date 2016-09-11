using Proxy.Helpers;
using Proxy.ServerEntities;
using Proxy.ServerEntities.Users;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Proxy
{
    public class DeMail
    {
        private ConcurrentDictionary<UserManager, ConcurrentQueue<ServerMessage>> mailbox = new ConcurrentDictionary<UserManager, ConcurrentQueue<ServerMessage>>();
        private ConcurrentList usersList = new ConcurrentList();
        public AsteriskEntity Asterisk;

        public void AddUser(UserManager user)
        {
            if (mailbox.ContainsKey(user))
            {
                return;
            }
            ConcurrentQueue<ServerMessage> messages = new ConcurrentQueue<ServerMessage>();
            mailbox.AddOrUpdate(user, messages, (key, oldValue) => messages);
            usersList.Add(user);
        }
        public void AddAsteriskConnection(AsteriskEntity asterisk)
        {
            Asterisk = asterisk;
            ConcurrentQueue<ServerMessage> messages = new ConcurrentQueue<ServerMessage>();
            mailbox.AddOrUpdate(asterisk, messages, (key, oldValue) => messages);
        }
        public void DeleteUser(UserManager user)
        {
            usersList.Remove(user);
            ConcurrentQueue<ServerMessage> ignored;
            mailbox.TryRemove(user, out ignored);
        }
        public void Clear()
        {
            ///Дописать лок аглок для списков и удалить ключи с массивами из словаря
            usersList.Clear();
        }
        public List<UserManager> GetUsers()
        {
            return usersList.ToList();
        }

        public void SendMessage(ServerMessage message)
        {
            switch (message.type)
            {
                case MessageType.AsteriskAction:
                    if (Asterisk != null)
                    {
                        mailbox[Asterisk].Enqueue(message);
                    }
                    break;
                case MessageType.AsteriskMessage:
                    foreach (var user in usersList)
                    {
                        mailbox[user].Enqueue(message);
                    }
                    break;
                case MessageType.ChatMessage:
                    break;
                case MessageType.InnerMessage:
                    break;
                case MessageType.SqlMessage:
                    break;
            }
        }
        public List<ServerMessage> GrabMessages(UserManager entity)
        {
            if (!mailbox.ContainsKey(entity))
            {
                ///ДОПИЛИТЬ ЭКЗЕБЖОН
                return null;
            }
            List<ServerMessage> temp = new List<ServerMessage>();
            ServerMessage item;
            while (!mailbox[entity].IsEmpty)
            {
                if (mailbox[entity].TryDequeue(out item))
                {
                    temp.Add(item);
                }
            }
            return temp;
        }
    }
}
