using System;
using System.Collections.Generic;
using System.Text;
using HiganTour.Components;
using Lichen.Entities;
using Lichen.Entities.Components;

namespace HiganTour.Systems
{
    public class HiganRenderSystem : Lichen.Entities.System
    {
        RenderSystem renderSystem = new RenderSystem();

        public override void Update(Scene scene)
        {
            List<WindyComponent> windyComponents = scene.GetComponentGroup<WindyComponent>().List;

            foreach (WindyComponent windyComponent in windyComponents)
            {
                windyComponent.OldX = windyComponent.Owner.CumulativeX;
                windyComponent.OldY = windyComponent.Owner.CumulativeY;
                windyComponent.Owner.CumulativeX += (float)windyComponent.X2;
                windyComponent.Owner.CumulativeY += (float)windyComponent.Y2;
            }

            renderSystem.Update(scene);

            foreach (WindyComponent windyComponent in windyComponents)
            {
                windyComponent.Owner.CumulativeX = (float)windyComponent.OldX;
                windyComponent.Owner.CumulativeY = (float)windyComponent.OldY;
            }
        }
    }
}
