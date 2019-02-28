using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;
using HiganTour.Components;

namespace HiganTour.Systems
{
    // TODO: Consider having a special ComponentGroup format that pre-filters invisible entities.
    public class RenderSystem : Lichen.Entities.System
    {
        public override void Update(Scene scene)
        {
            //base.Update();
            ComponentGroup<SpriteComponent> sprites = scene.GetComponentGroup<SpriteComponent>();
            ComponentGroup<TextComponent> texts = scene.GetComponentGroup<TextComponent>();
            List<RenderElement> renderList = new List<RenderElement>();

            foreach (SpriteComponent sprite in sprites.List)
            {
                if (sprite.Owner.Visible == false) continue;
                // TODO: Also check if sprite is offscreen.
                //sprite.Update();
                renderList.Add(new RenderElement(sprite.Owner, sprite.Update));
            }
            foreach (TextComponent text in texts.List)
            {
                if (text.Owner.Visible == false) continue;
                //text.Update();
                renderList.Add(new RenderElement(text.Owner, text.Update));
            }

            renderList.Sort(new RenderElementSorter());

            foreach (RenderElement element in renderList)
            {
                if(element.Entity.TryGetComponent(out RenderOffsetComponent offsetComponent))
                {
                    float x = element.Entity.CumulativeX;
                    float y = element.Entity.CumulativeY;
                    element.Entity.OverrideCumulativePosition(x + offsetComponent.X, y + offsetComponent.Y);
                    element.Action();
                    element.Entity.OverrideCumulativePosition(x, y);
                }
                else
                {
                    element.Action();
                }
            }
        }
    }

    public class RenderElement
    {
        public Entity Entity{ get; set; }
        public Action Action { get; set; }
        public int Layer { get; set; }
        public float Depth { get; set; }
        public int AutoDepth { get; set; }

        public RenderElement(Entity entity, Action action)
        {
            Entity = entity;
            Action = action;
            Layer = entity.RenderLayer;
            Depth = entity.RenderDepth;
            AutoDepth = entity.AutoDepth;
        }
    }

    public class RenderElementSorter : IComparer<RenderElement>
    {
        public int Compare(RenderElement e1, RenderElement e2)
        {
            int compared = e1.Layer.CompareTo(e2.Layer);
            if (compared != 0) return compared;
            compared = e1.Depth.CompareTo(e2.Depth);
            if (compared != 0) return compared;
            return e1.AutoDepth.CompareTo(e2.AutoDepth);
        }
    }
}
