using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lichen.Entities;

namespace LifeDeath.Components
{
    class WindyComponent : Lichen.Entities.Component, Lichen.Entities.IUpdateComponent
    {
        double x, y;
        double dx, dy;
        double vx, vy;
        double ax, ay;
        double theta;
        Random rand;

        public WindyComponent(float x, float y)
        {
            this.x = x;
            this.y = y;
            rand = new Random(Lichen.GlobalServices.GlobalRandom.Next());
        }

        public void Update()
        {
            vx += rand.NextDouble() - 0.5d;
            vy += rand.NextDouble() - 0.5d;
            dx += vx;
            dy += vy;
            vx *= 0.9d;
            vy *= 0.9d;
            dx *= 0.9d;
            dy *= 0.9d;

            /*
            theta += (rand.NextDouble() - 0.5d) * 0.8d;
            ax = Math.Cos(theta) * 20d;
            ay = Math.Sin(theta) * 20d;
            dx = (10d * dx + ax) / 11d;
            dy = (10d * dy + ay) / 11d;
            */

            //dx = 0f;
            //dy = 0f;

            Entity nearestActor = null;
            double nearestDistance = 1000d;
            foreach (Entity entity in Owner.ActorList)
            {
                double distance = Math.Sqrt(Math.Pow(entity.X - x, 2) + Math.Pow((entity.Y - y) * 2d, 2));
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestActor = entity;
                }
            }
            if (nearestActor != null && nearestDistance < 200d)
            {
                double multiplier = (200d - nearestDistance) / 4d;
                dx = -(nearestActor.X - x) / nearestDistance * multiplier;
                dy = -(nearestActor.Y - y) * 2f / nearestDistance * multiplier;
            }

            Owner.X = (float)(x + dx);
            Owner.Y = (float)(y + dy);
        }
    }
}
