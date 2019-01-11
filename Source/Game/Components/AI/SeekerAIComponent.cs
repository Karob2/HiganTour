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
        Entity target;

        public SeekerAIComponent(Entity target)
        {
            this.target = target;
        }

        public void Update()
        {
            Vector2 vector = new Vector2(target.X - Owner.X, target.Y - Owner.Y);
            vector.Normalize();
            Owner.X += vector.X;
            Owner.Y += vector.Y;
        }
    }
}
