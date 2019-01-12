using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lichen.Entities;
using Microsoft.Xna.Framework;

namespace LifeDeath.Components.AI
{
    class BulletComponent : Lichen.Entities.Component, Lichen.Entities.IUpdateComponent
    {
        Scenes.Level level;
        float x, y, vx, vy;

        public BulletComponent(Scenes.Level level, float x, float y, float vx, float vy)
        {
            this.level = level;
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
        }

        public void Set(float x, float y, float vx, float vy)
        {
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
        }

        public void Reset()
        {
            x = -200;
            y = 0;
            vx = 0;
            vy = 0;
        }

        public void Update()
        {
            x += vx;
            y += vy;

            Owner.X = x;
            Owner.Y = y;

            if (Math.Abs(x - level.Player.X) > 1500 || Math.Abs(y - level.Player.Y) > 1500)
            {

            }
        }
    }
}
