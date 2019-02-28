using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;

namespace HiganTour.Components
{
    public class RenderOffsetComponent : Component
    {
        public float X { get; set; }
        public float Y { get; set; }

        public override void AttachTo(Entity entity)
        {
            entity.AddComponent(this);
        }
    }
}
