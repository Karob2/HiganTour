using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lichen.Entities;
using Microsoft.Xna.Framework;

namespace LifeDeath.Components.AI
{
    class SeekerAIComponent : Lichen.Entities.Component, Lichen.Entities.IUpdateComponent
    {
        Scenes.Level level;
        Entity target;

        public SeekerAIComponent(Scenes.Level level, Entity target)
        {
            this.level = level;
            this.target = target;
        }

        public void Update()
        {
            if (!level.Hiding)
            {
                Vector2 vector = new Vector2(target.X - Owner.X, target.Y - Owner.Y);
                vector.Normalize();
                Owner.X += vector.X;
                Owner.Y += vector.Y;
            }
        }
    }
}
