using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;
using HiganTour.Components;

namespace HiganTour.Systems
{
    public class MainMenuSystem : Lichen.Entities.System
    {
        public override void Update(Scene scene)
        {
            // TODO: Should I expect there to only be one MenuComponent, and thus simplify this to a single constant reference somehow?
            ComponentGroup<MenuComponent> components = scene.GetComponentGroup<MenuComponent>();
            foreach (MenuComponent component in components.List)
            {
                component.Update();
            }
        }
    }
}
