using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeDeath.Components
{
    class WindyComponent : Lichen.Entities.Component, Lichen.Entities.IUpdateComponent
    {
        float x, y;
        float dx, dy;
        float vx, vy;
        float ax, ay;
        Random rand;

        public WindyComponent(float x, float y)
        {
            this.x = x;
            this.y = y;
            rand = new Random(Lichen.GlobalServices.GlobalRandom.Next());
        }

        public void Update()
        {
            vx += (float)rand.NextDouble() - 0.5f;
            vy += (float)rand.NextDouble() - 0.5f;
            dx += vx;
            dy += vy;
            vx *= 0.9f;
            vy *= 0.9f;
            dx *= 0.9f;
            dy *= 0.9f;
            Owner.X = x + dx;
            Owner.Y = y + dy;
        }
    }
}
