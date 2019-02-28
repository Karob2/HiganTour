using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities
{
    public class Component
    {
        public Entity Owner { get; set; }

        public Component Clone()
        {
            return (Component)this.MemberwiseClone();
        }
    }

    public class ComponentGroup
    {

    }

    public class ComponentGroup<T> : ComponentGroup where T : Component
    {
        // TODO: Replace this with a re-usable smart collection?
        List<T> list = new List<T>();
        public List<T> List { get { return list; } }

        public void Add(T component, out int id)
        {
            id = list.Count;
            list.Add(component);
        }
    }
}
