using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities
{
    public abstract class Component
    {
        public Entity Owner { get; set; }

        public virtual void OnAttach() { }

        public abstract void AttachTo(Entity entity);

        public virtual void FilterComponent() { }

        public Component Clone()
        {
            return (Component)this.MemberwiseClone();
        }
    }
}
