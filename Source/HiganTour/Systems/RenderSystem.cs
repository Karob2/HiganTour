using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;

namespace HiganTour.Systems
{
    public class RenderSystem : Lichen.Entities.System
    {
        public override void Update(Scene scene)
        {
            //base.Update();
            ComponentGroup<SpriteComponent> sprites = scene.GetComponentGroup<SpriteComponent>();
            ComponentGroup<TextComponent> texts = scene.GetComponentGroup<TextComponent>();
            foreach (SpriteComponent sprite in sprites.List)
            {
                if (sprite.Owner.Visible == true)
                    sprite.Update();
            }
            foreach (TextComponent text in texts.List)
            {
                if (text.Owner.Visible == true)
                    text.Update();
            }
        }
    }
}
