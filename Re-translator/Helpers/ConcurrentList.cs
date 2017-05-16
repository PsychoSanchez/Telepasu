using Proxy.ServerEntities;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

namespace Proxy.Helpers
{
    class ConcurrentList : IList<EntityManager>
    {
        ManualResetEvent mutex = new ManualResetEvent(true);
        List<EntityManager> list = new List<EntityManager>();
        public EntityManager this[int index]
        {
            get
            {
                return list[index];
            }

            set
            {
                list[index] = value;
            }
        }

        public int Count
        {
            get
            {
                mutex.WaitOne();
                int length = list.Count;
                mutex.Set();
                return length;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(EntityManager item)
        {
            mutex.WaitOne();
            list.Add(item);
            mutex.Set();
        }

        public void Clear()
        {
            mutex.WaitOne();
            list.Clear();
            mutex.Set();
        }

        public bool Contains(EntityManager item)
        {
            mutex.WaitOne();
            bool temp = list.Contains(item);
            mutex.Set();
            return temp;
        }

        public void CopyTo(EntityManager[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<EntityManager> GetEnumerator()
        {
            mutex.WaitOne();
            IEnumerator<EntityManager> temp = list.GetEnumerator();
            mutex.Set();
            return temp;
        }

        public int IndexOf(EntityManager item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, EntityManager item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(EntityManager item)
        {
            mutex.WaitOne();
            bool temp = list.Remove(item);
            mutex.Set();
            return temp;
        }

        public void RemoveAt(int index)
        {
            mutex.WaitOne();
            list.RemoveAt(index);
            mutex.Set();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            mutex.WaitOne();
            IEnumerator<EntityManager> temp = list.GetEnumerator();
            mutex.Set();
            return temp;
        }
        public List<EntityManager> ToList()
        {
            mutex.WaitOne();
            List<EntityManager> temp = list;
            mutex.Set();
            return temp;
        }
        //public delegate void ActionFunction();
        //public void WorkAction(ActionFunction a)
        //{
        //    mutex.WaitOne();
        //    a();
        //    mutex.Set();
        //}
    }
}
