using Proxy.Helpers;
using Proxy.ServerEntities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using Proxy.Messages.API.Admin;

namespace Proxy
{
    public class MailPost
    {
        private const string ModuleName = "#(Subscribers Mail Post) ";
        /// <summary>
        /// Key - Tag (string)
        /// </summary>
        private readonly ConcurrentDictionary<string, ConcurrentList> _subscribers = new ConcurrentDictionary<string, ConcurrentList>();
        private readonly ConcurrentDictionary<EntityManager, List<string>> _subscriptions = new ConcurrentDictionary<EntityManager, List<string>>();
        private readonly List<MethodCall> _modulesInfo = new List<MethodCall>();
        private readonly List<MethodCall> _iinnerModulesInfo = new List<MethodCall>();
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

        public void AddModule(string uid, EntityManager entity, AddModuleMethod data)
        {
            var subs = _subscribers.GetOrAdd(uid, new ConcurrentList());
            subs.Add(entity);
            _moduleList.Add(entity);
            _modulesInfo.Add(data);
        }

        struct ModulesInfo
        {
            public string Modules;
            public string NativeModules;
        }
        public string GetConnectedModules()
        {
            return JsonConvert.SerializeObject(new ModulesInfo()
            {
                Modules = JsonConvert.SerializeObject(_modulesInfo),
                NativeModules = JsonConvert.SerializeObject(_iinnerModulesInfo)
            });
        }

        public void Subscribe(EntityManager entity, string tag)
        {
            telepasu.log(ModuleName + entity.UserName + " subscrived to tag: " + tag);
            var subscribers = _subscribers.GetOrAdd(tag, new ConcurrentList());
            subscribers.Add(entity);
            var entitySubs = _subscriptions.GetOrAdd(entity, new List<string>());
            entitySubs.Add(tag);
        }
        public void Unsubscribe(EntityManager entity, string tag)
        {
            var subs = _subscribers.GetOrAdd(tag, new ConcurrentList());
            subs.Remove(entity);
            var entitySubs = _subscriptions.GetOrAdd(entity, new List<string>());
            entitySubs.Remove(tag);
        }
        public void Unsubscribe(EntityManager entity)
        {
            var subs = _subscriptions.GetOrAdd(entity, new List<string>());
            foreach (var sub in subs)
            {
                var entitySubs = _subscribers.GetOrAdd(sub, new ConcurrentList());
                entitySubs.Remove(entity);
            }
            var a = new List<string>();
            while (_subscriptions.TryRemove(entity, out a))
            {
                Thread.Sleep(5);
            }
        }
        public List<string> GetSubscribtions(EntityManager entity)
        {
            var entitySubs = _subscriptions.GetOrAdd(entity, new List<string>());
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
