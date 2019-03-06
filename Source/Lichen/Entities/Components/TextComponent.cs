﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Lichen.Entities.Components
{
    public class TextComponent : RenderComponent
    {
        public Libraries.Font Font { get; set; }
        public Color Color { get; set; }
        public string Value { get; set; }

        public TextComponent(Libraries.Font font, string value)
        {
            Font = font;
            Value = value;
            Color = Color.White;
        }

        public override void Render()
        {
            if (Font != null)
            {
                Font.Render(Value, Owner.CumulativeX, Owner.CumulativeY, Color);
            }
        }

        public override void AttachTo(Entity entity)
        {
            entity.AddComponent(this);
        }
    }
}
