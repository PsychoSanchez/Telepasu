using Proxy.ServerEntities;
using Proxy.ServerEntities.UserEntities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    public class DeMail
    {
        private ConcurrentList usersList = new ConcurrentList();
        public AsteriskEntity Asterisk;
        private ConcurrentDictionary<ServerEntity, ConcurrentQueue<ServerMessage>> mailbox = new ConcurrentDictionary<ServerEntity, ConcurrentQueue<ServerMessage>>();

        public void AddUser(ServerEntity user)
        {
            if (mailbox.ContainsKey(user))
            {
                return;
            }
            ConcurrentQueue<ServerMessage> messages = new ConcurrentQueue<ServerMessage>();
            mailbox.AddOrUpdate(user, messages, (key, oldValue) => messages);
            usersList.Add(user);
        }
        public void DeleteUser(ServerEntity user)
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
        public List<ServerEntity> GetUsers()
        {
            return usersList.ToList();
        }

        public void SendMessage(ServerMessage message)
        {

        }
        public List<ServerMessage> GrabMessages(ServerEntity entity)
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
                mailbox[entity].TryDequeue(out item);
                temp.Add(item);
            }
            return temp;
        }
    }
}
