using Proxy.Helpers;
using Proxy.ServerEntities;
using System.Collections.Concurrent;

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
            telepasu.log(ModuleName + entity.UserName + " subscrived to tag: "+ tag);
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
