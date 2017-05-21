using Proxy.Helpers;
using Proxy.ServerEntities;
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
        private readonly ConcurrentDictionary<string, List<string>> _subscriptions = new ConcurrentDictionary<string, List<string>>();
        private readonly ConcurrentList _moduleList = new ConcurrentList();
        private readonly ConcurrentList _appsList = new ConcurrentList();
        private readonly ConcurrentList _innerModules = new ConcurrentList();


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
            telepasu.log(ModuleName + entity.UserName + " subscrived to tag: " + tag);
            var subscribers = _subscribers.GetOrAdd(tag, new ConcurrentList());
            subscribers.Add(entity);
            var entitySubs = _subscriptions.GetOrAdd(entity.UserName, new List<string>());
            entitySubs.Add(tag);
        }
        public void Unsubscrive(EntityManager entity, string tag)
        {
            var subs = _subscribers.GetOrAdd(tag, new ConcurrentList());
            subs.Remove(entity);
            var entitySubs = _subscriptions.GetOrAdd(entity.UserName, new List<string>());
            entitySubs.Remove(tag);
        }
        public List<string> GetSubscribtions(EntityManager entity)
        {
            var entitySubs = _subscriptions.GetOrAdd(entity.UserName, new List<string>());
            return entitySubs;
        }
        public void PostMessage(ServerMessage message)
        {
            if (message.Tag == null || !_subscribers.ContainsKey(message.Tag))
            {
                return;
            }
            var subscribers = _subscribers[message.Tag];
            foreach (EntityManager subscriber in subscribers)
            {
                subscriber.SendMesage(message);
            }
        }
    }
}
