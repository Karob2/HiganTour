﻿using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;
using HiganTour.Components;
using System.Linq;

namespace HiganTour.Systems
{
    public class WindySystem : Lichen.Entities.System
    {
        Random rand = new Random();
        Entity camera;
        List<Entity> actorList;

        public override void Update(Scene scene)
        {
            ComponentGroup<WindyComponent> components = scene.GetComponentGroup<WindyComponent>();
            camera = scene.GetEntity("camera");
            //CameraComponent cameraComponent = scene.GetComponentGroup<CameraComponent>().List.FirstOrDefault();
            //camera = cameraComponent.Owner;
            actorList = scene.GetGroup("movegrass");

            foreach (WindyComponent component in components.List)
            {
                Update2(scene, component);
            }
        }

        public void Update2(Scene scene, WindyComponent component)
        {
            /*
            float pos = 0;
            if (camera != null) pos = camera.Y;

            float dist = (float)component.Y + pos + 200f;
            if (dist >= 0) component.Y = dist % 920f - pos - 200f;
            else component.Y = (920f + dist % 920f) - pos - 200f;
            */

            if (!component.Owner.TryGetComponent(out RenderOffsetComponent offsetComponent))
            {
                offsetComponent = new RenderOffsetComponent();
                component.Owner.AddComponent(offsetComponent);
            }

            component.Vx += rand.NextDouble() - 0.5d;
            component.Vy += rand.NextDouble() - 0.5d;
            component.Dx += component.Vx;
            component.Dy += component.Vy;
            component.Vx *= 0.9d;
            component.Vy *= 0.9d;
            component.Dx *= 0.9d;
            component.Dy *= 0.9d;

            double ddx = 0f;
            double ddy = 0f;

            if (actorList != null)
            {
                double x = component.Owner.X + component.X2;
                double y = component.Owner.Y + component.Y2;
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

            component.X2 = (10d * component.X2 + component.Dx + ddx) / 11d;
            component.Y2 = (10d * component.Y2 + component.Dy + ddy) / 11d;
            //component.Owner.X = (float)(component.X + component.X2);
            //component.Owner.Y = (float)(component.Y + component.Y2);
            offsetComponent.X = (float)component.X2;
            offsetComponent.Y = (float)component.Y2;

            //Owner.X = (float)(x + dx + ddx);
            //Owner.Y = (float)(y + dy + ddy);
        }
    }
}