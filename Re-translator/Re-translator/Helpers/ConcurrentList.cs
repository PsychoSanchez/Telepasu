using Proxy.ServerEntities;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

namespace Proxy.Helpers
{
    class ConcurrentList : IList<UserManager>
    {
        ManualResetEvent mutex = new ManualResetEvent(true);
        List<UserManager> list = new List<UserManager>();
        public UserManager this[int index]
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

        public void Add(UserManager item)
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

        public bool Contains(UserManager item)
        {
            mutex.WaitOne();
            bool temp = list.Contains(item);
            mutex.Set();
            return temp;
        }

        public void CopyTo(UserManager[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<UserManager> GetEnumerator()
        {
            mutex.WaitOne();
            IEnumerator<UserManager> temp = list.GetEnumerator();
            mutex.Set();
            return temp;
        }

        public int IndexOf(UserManager item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, UserManager item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(UserManager item)
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
            IEnumerator<UserManager> temp = list.GetEnumerator();
            mutex.Set();
            return temp;
        }
        public List<UserManager> ToList()
        {
            mutex.WaitOne();
            List<UserManager> temp = list;
            mutex.Set();
            return temp;
        }
        //public delegate void ActionFunction();
        //public void DoAction(ActionFunction a)
        //{
        //    mutex.WaitOne();
        //    a();
        //    mutex.Set();
        //}
    }
}
