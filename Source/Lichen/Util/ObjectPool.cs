using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lichen.Util
{
    public class ObjectPool<T> where T : IPoolable,new()
    {
        HashSet<T> list; // HashSet instead of List or LinkedList for speedy pruning of excess vacancies.
        Stack<T> vacant; // Can replace with concurrent bag.

        public ObjectPool()
        {
            list = new HashSet<T>();
            vacant = new Stack<T>();
        }

        public T New()
        {
            T item;
            if (vacant.Count == 0)
            {
                item = new T();
                list.Add(item);
            }
            else
            {
                item = vacant.Pop();
                item.Vacant = false;
                item.Reset();
            }
            return item;
        }

        public void Free(T item)
        {
            //if (item == null || item.Vacant) return;
            /*
            // If the pool is mostly vacant, delete (dereference) the item instead of keeping for re-use.
            if (vacant.Count > 128 && vacant.Count > list.Count * 7 / 8)
            {
                list.Remove(item);
                //item.Vacant = true;
            }
            */
            vacant.Push(item);
            item.Vacant = true;
            // If the pool is mostly vacant, aggressively prune the vacancies.
            if (vacant.Count > 128 && vacant.Count > list.Count * 7 / 8)
            {
                // Deletes half of the vacancies.
                int removeCount = vacant.Count / 2;
                for (int i = 0; i < removeCount; i++)
                {
                    list.Remove(vacant.Pop());
                }
            }
        }
    }
}
