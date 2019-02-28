using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;
using HiganTour.Components;

namespace HiganTour.Systems
{
    public class RenderOffsetSubsystem : Subsystem
    {
        public override void Update(LinkedListNode<Subsystem> node, Entity entity, Action action)
        {
            if (entity.TryGetComponent(out RenderOffsetComponent offsetComponent))
            {
                float x = entity.CumulativeX;
                float y = entity.CumulativeY;
                entity.OverrideCumulativePosition(x + offsetComponent.X, y + offsetComponent.Y);
                base.Update(node, entity, action);
                entity.OverrideCumulativePosition(x, y);
            }
            else
            {
                base.Update(node, entity, action);
            }
        }
    }
}
