using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;
using HiganTour.Components;
using System.Linq;
using Microsoft.Xna.Framework;

namespace HiganTour.Systems
{
    public class WindySystem : Lichen.Entities.System
    {
        Random rand = new Random();

        public override void Update(Scene scene)
        {
            ComponentGroup<WindyComponent> components = scene.GetComponentGroup<WindyComponent>();
            Entity camera = scene.GetEntity("camera");
            //CameraComponent cameraComponent = scene.GetComponentGroup<CameraComponent>().List.FirstOrDefault();
            //camera = cameraComponent.Owner;
            List<Entity> actorList = scene.GetGroup("movegrass");
            ComponentGroup<BodyComponent> bodyComponents = scene.GetComponentGroup<BodyComponent>();

            foreach (WindyComponent component in components.List)
            {
                // Endless scrolling flowers.
                if (!bodyComponents.TryGetByOwner(component.Owner, out BodyComponent bodyComponent))
                {
                    bodyComponent = new BodyComponent();
                    component.Owner.AddComponent(bodyComponent);
                    bodyComponent.Position = new Vector3(component.Owner.X, component.Owner.Y, 0);
                }
                float pos = 0;
                float newY;
                if (camera != null) pos = camera.Y;

                float dist = bodyComponent.Position.Y + pos + 200f;
                if (dist >= 0) newY = dist % 920f - pos - 200f;
                else newY = (920f + dist % 920f) - pos - 200f;
                bodyComponent.Position = new Vector3(bodyComponent.Position.X, newY, bodyComponent.Position.Z);

                // Swaying.
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
                    //Entity nearestActor = null;
                    //double nearestDistance = 1000d;
                    double count = 0d;

                    foreach (Entity entity in actorList)
                    {
                        double hiding = 1d;
                        if (entity.HasTag("hiding")) hiding = -1d;
                        double distance = Math.Sqrt(Math.Pow(entity.X - x, 2) + Math.Pow((entity.Y - y) * 2d, 2));
                        /*
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestActor = entity;
                        }
                        */
                        if (distance < 200d)
                        {
                            double multiplier = (200d - distance) / 2d * hiding;
                            //if (hiding < 0) multiplier = Math.Min(multiplier, distance);
                            double influence = 200d - distance;

                            if (distance == 0 && hiding > 0)
                            {
                                ddy += multiplier * influence;
                            }
                            else
                            {
                                if (distance < 1d) distance = 1d;
                                ddx += -(entity.X - x) / distance * multiplier * influence;
                                ddy += -(entity.Y - y) / distance * multiplier * influence;
                            }
                            count += influence;
                        }
                    }

                    if (count > 0d)
                    {
                        ddx /= count;
                        ddy /= count;
                    }

                    /*
                    if (nearestActor != null && nearestDistance < 200d)
                    {
                        double multiplier = (200d - nearestDistance) / 4d;

                        if (nearestDistance == 0)
                        {
                            ddx = 0d;
                            ddy = 2d * multiplier;
                        }
                        else
                        {
                            if (nearestDistance < 1d) nearestDistance = 1d;
                            ddx = -(nearestActor.X - x) / nearestDistance * multiplier;
                            ddy = -(nearestActor.Y - y) * 2d / nearestDistance * multiplier;
                        }

                        if (nearestActor.HasTag("hiding"))
                        {
                            ddx = -ddx;
                            ddy = -ddy;
                        }
                    }
                    */
                }

                component.X2 = (10d * component.X2 + component.Dx + ddx) / 11d;
                component.Y2 = (10d * component.Y2 + component.Dy + ddy) / 11d;
                //component.Owner.X = (float)(component.X + component.X2);
                //component.Owner.Y = (float)(component.Y + component.Y2);
                /*
                offsetComponent.X = (float)component.X2;
                offsetComponent.Y = (float)component.Y2;
                */

                //Owner.X = (float)(x + dx + ddx);
                //Owner.Y = (float)(y + dy + ddy);
            }
        }
    }
}
