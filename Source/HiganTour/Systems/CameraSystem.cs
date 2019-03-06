using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;
using HiganTour.Components;

namespace HiganTour.Systems
{
    public class CameraSystem : Lichen.Entities.System
    {
        public override void Update(Scene scene)
        {
            ComponentGroup<CameraComponent> components = scene.GetComponentGroup<CameraComponent>();
            foreach (CameraComponent component in components.List)
            {
                Entity target = component.Owner.Scene.GetEntity(component.TargetName);
                if (target == null) return;
                //Owner.X = -target.X + Lichen.GlobalServices.Game.GraphicsDevice.Viewport.Width / 2;
                component.Owner.X = 0;
                component.Owner.Y = -target.Y + Lichen.GlobalServices.Game.GraphicsDevice.Viewport.Height / 2f + 100f;
            }
        }
    }
}
