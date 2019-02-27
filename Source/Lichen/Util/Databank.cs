using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Util
{
    public class Databank
    {
        Dictionary<string, bool> tags;
        Dictionary<string, LinkedList<bool>> booleans;
        Dictionary<string, int> integers;
        Dictionary<string, float> floats;

        public T GetValue<T>(string name, Dictionary<string, T> dic)
        {
            if (dic == null) return default(T);
            return dic.TryGetValue(name, out T output) ? output : default(T);
        }

        public T GetFirstValue<T>(string name, Dictionary<string, LinkedList<T>> dic)
        {
            return GetValue(name, dic).First.Value;
        }

        public void SetValue<T>(string name, T value, Dictionary<string, T> dic)
        {
            if (dic == null)
            {
                dic = new Dictionary<string, T>();
                dic.Add(name, value);
            }
            else
            {
                dic[name] = value;
            }
        }

        public void SetFirstValue<T>(string name, T value, Dictionary<string, LinkedList<T>> dic)
        {
            LinkedList<T> list = GetValue(name, dic);
            if (list == null)
            {
                list = new LinkedList<T>();
                list.AddFirst(value);
            }
            else if (list.Count == 0) list.AddFirst(value);
            else if (list.Count == 1) list.First.Value = value;
            else
            {
                list.Clear();
                list.AddFirst(value);
            }
            SetValue(name, list, dic);
        }

        public LinkedList<T> GetList<T>(string name, Dictionary<string, LinkedList<T>> dic)
        {
            LinkedList<T> list = GetValue(name, dic);
            if (list == null)
            {
                list = new LinkedList<T>();
                SetValue(name, list, dic);
            }
            return list;
        }

        // Should I keep tags in Entity instead?
        /*
        public bool HasTag(string name)
        {
            return GetValue(name, tags);
        }
        public void SetTag(string name, bool value)
        {
            SetValue(name, value, tags);
        }
        public void AddTag(string name)
        {
            SetValue(name, true, tags);
        }
        public void RemoveTag(string name)
        {
            SetValue(name, false, tags);
        }
        */

        // TODO: The downside of these value types is that components cannot hold a reference - they have to lookup each time they wish to read or change the value. (Though in many cases the object would be too temporary or numerous to be worth holding a reference for.)

        // Here I've made an example where the datatype can be used for singles or for lists of data.
        public bool GetBool(string name)
        {
            return GetFirstValue(name, booleans);
        }
        public void SetBool(string name, bool value)
        {
            SetFirstValue(name, value, booleans);
        }
        public LinkedList<bool> GetBoolList(string name)
        {
            return GetList(name, booleans);
        }

        public int GetInt(string name)
        {
            return GetValue(name, integers);
        }

        public void SetInt(string name, int value)
        {
            SetValue(name, value, integers);
        }

        public float GetFloat(string name)
        {
            return GetValue(name, floats);
        }

        public void SetFloat(string name, float value)
        {
            SetValue(name, value, floats);
        }
    }
}
