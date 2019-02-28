using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities
{
    public abstract class RenderComponent : Component
    {
        public abstract void Render();

        public override void FilterComponent()
        {
            //base.FilterComponent();
            Owner.Scene.FilterComponent<RenderComponent>(this);
        }
    }
}
