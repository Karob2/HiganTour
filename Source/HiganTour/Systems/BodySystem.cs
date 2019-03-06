using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;
using HiganTour.Components;

namespace HiganTour.Systems
{
    public class BodySystem : Lichen.Entities.System
    {
        public override void Update(Scene scene)
        {
            ComponentGroup<BodyComponent> components = scene.GetComponentGroup<BodyComponent>();
            foreach (BodyComponent component in components.List)
            {
                component.Position += component.Velocity;

                component.Owner.X = component.Position.X;
                component.Owner.Y = component.Position.Y;
                component.Owner.RenderDepth = component.Owner.Y;
            }
        }
    }
}
