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
                //if (component.Owner.TryGetComponent(out BodyComponent bodyComponent))
                //{
                    component.Owner.RenderDepth = component.Owner.Y;
                //}
            }
        }
    }
}
