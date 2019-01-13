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
        int bulletTimer;

        public SeekerAIComponent(Scenes.Level level)
        {
            this.level = level;
            target = level.Player;
        }

        public void Update()
        {
            if (!level.Hiding && Math.Abs(Owner.Y - level.Player.Y) < 400)
            {
                Vector2 vector = new Vector2(target.X - Owner.X, target.Y - Owner.Y);
                if (vector.X != 0 || vector.Y != 0)
                {
                    vector.Normalize();
                    Owner.X += vector.X;
                    Owner.Y += vector.Y;

                    bulletTimer++;
                    if (bulletTimer >= 100)
                    {
                        level.MakeBullet(Owner.X, Owner.Y, vector.X * 10f, vector.Y * 10f);
                        bulletTimer = 0;
                    }
                }
            }

            if (Math.Abs(Owner.Y - level.Player.Y) > 720)
            {
                Owner.X = -200;
                Owner.Y = 0;
                Owner.Active = false;
                Owner.Visible = false;
            }
        }
    }
}
