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
}
