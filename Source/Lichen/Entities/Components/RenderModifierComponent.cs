using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities.Components
{
    public abstract class RenderModifierComponent : Component
    {
        public abstract void Update();

        public override void FilterComponent()
        {
            //base.FilterComponent();
            Owner.Scene.FilterComponent<RenderModifierComponent>(this);
        }
    }
}
