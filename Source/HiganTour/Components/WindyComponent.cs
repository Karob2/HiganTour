﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lichen.Entities;

namespace HiganTour.Components
{
    class WindyComponent : Lichen.Entities.Component, Lichen.Entities.IUpdateComponent
    {
        double x, y;
        double dx, dy;
        double vx, vy;
        double ax, ay;
        double theta;
        double xx, yy;
        Random rand;

        public WindyComponent(float x, float y)
        {
            this.x = x;
            this.y = y;
            rand = new Random(Lichen.GlobalServices.GlobalRandom.Next());
        }

        public void Update()
        {
            Entity camera = null;
            List<Entity> cam = Owner.Scene.GetGroupList("camera");
            if (cam != null) camera = cam.ElementAtOrDefault(0);

            float pos = 0;
            if (camera != null) pos = camera.Y;

            float dist = (float)y + pos + 200f;
            if (dist >= 0) y = dist % 920f - pos - 200f;
            else y = (920f + dist % 920f) - pos - 200f;
            /*
            if (dist > 920f)
            {
                y -= 920d;
            }
            if (dist < 0f)
            {
                y += 920d;
            }
            */

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

            double ddx = 0f;
            double ddy = 0f;
            List<Entity> actorList = null;
            // TODO: Super wasteful repeating this for every flower for every frame? Maybe not?
            if (Owner.Scene != null) actorList = Owner.Scene.GetGroupList("movegrass");

            if (actorList != null)
            {
                Entity nearestActor = null;
                double nearestDistance = 1000d;

                foreach (Entity entity in actorList)
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
                    ddx = -(nearestActor.X - x) / nearestDistance * multiplier;
                    ddy = -(nearestActor.Y - y) * 2f / nearestDistance * multiplier;

                    if (nearestActor.HasTag("hiding"))
                    {
                        ddx = -ddx;
                        ddy = -ddy;
                    }
                }
            }

            xx = (10d * xx + dx + ddx) / 11d;
            yy = (10d * yy + dy + ddy) / 11d;
            Owner.X = (float)(x + xx);
            Owner.Y = (float)(y + yy);

            //Owner.X = (float)(x + dx + ddx);
            //Owner.Y = (float)(y + dy + ddy);
        }
    }
}
