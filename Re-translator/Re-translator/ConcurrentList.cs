using Proxy.ServerEntities;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

namespace Proxy
{
    class ConcurrentList : IList<ServerEntity>
    {
        ManualResetEvent mutex = new ManualResetEvent(true);
        List<ServerEntity> list = new List<ServerEntity>();
        public ServerEntity this[int index]
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

        public void Add(ServerEntity item)
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

        public bool Contains(ServerEntity item)
        {
            mutex.WaitOne();
            bool temp = list.Contains(item);
            mutex.Set();
            return temp;
        }

        public void CopyTo(ServerEntity[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ServerEntity> GetEnumerator()
        {
            mutex.WaitOne();
            IEnumerator<ServerEntity> temp = list.GetEnumerator();
            mutex.Set();
            return temp;
        }

        public int IndexOf(ServerEntity item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, ServerEntity item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ServerEntity item)
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
            IEnumerator<ServerEntity> temp = list.GetEnumerator();
            mutex.Set();
            return temp;
        }
        public List<ServerEntity> ToList()
        {
            mutex.WaitOne();
            List<ServerEntity> temp = list;
            mutex.Set();
            return temp;
        }
    }
}
