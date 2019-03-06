using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;
using Lichen.Entities.Components;

namespace HiganTour.Components
{
    public class RenderOffsetComponent : Component
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float OldX { get; set; }
        public float OldY { get; set; }

        /*
        public override void Update()
        {
            Owner.CumulativeX += X;
            Owner.CumulativeY += Y;
        }
        */

        public override void AttachTo(Entity entity)
        {
            entity.AddComponent(this);
        }
    }
    /*
    public class RenderOffsetComponent : Component
    {
        public float X { get; set; }
        public float Y { get; set; }

        public override void AttachTo(Entity entity)
        {
            entity.AddComponent(this);
        }
    }
    */
}
