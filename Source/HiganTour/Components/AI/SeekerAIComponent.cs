using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lichen.Entities;
using Microsoft.Xna.Framework;

namespace HiganTour.Components.AI
{
    class SeekerAIComponent : Lichen.Entities.Component, Lichen.Entities.IUpdateComponent
    {
        Scenes.Level level;
        Entity target;
        int bulletTimer;
        double theta;

        int mode;

        public SeekerAIComponent(Scenes.Level level)
        {
            this.level = level;
            target = level.Player;
        }

        public void SetAIMode(int location)
        {
            mode = location % (Math.Min(location / 4 + 1, 6)) + 1;
            //mode = location % 6 + 1;
            //mode = 6;
            bulletTimer = 0;
            theta = 0d;
        }

        public void Update()
        {
            if (!level.Hiding && Math.Abs(Owner.Y - level.Player.Y) < 600)
            {
                Vector2 vector = new Vector2(target.X - Owner.X, target.Y - Owner.Y);
                if (vector.X != 0 || vector.Y != 0)
                {
                    vector.Normalize();

                    if (mode == 1)
                    {
                        FacePlayer();
                        Owner.X += vector.X * 2f;
                        Owner.Y += vector.Y * 2f;
                    }

                    if (mode == 2)
                    {
                        FacePlayer();
                        bulletTimer++;
                        if (bulletTimer == 90 || bulletTimer == 80)
                        {
                            level.MakeBullet(Owner.X, Owner.Y, vector.X * 10f, vector.Y * 10f);
                        }
                        if (bulletTimer >= 100)
                        {
                            level.MakeBullet(Owner.X, Owner.Y, vector.X * 10f, vector.Y * 10f);
                            bulletTimer = 0;
                        }
                    }

                    if (mode == 3)
                    {
                        bulletTimer++;
                        if (bulletTimer >= 60)
                        {
                            Owner.X += vector.X * 10f;
                            Owner.Y += vector.Y * 10f;
                            FacePlayer();
                        }
                        else
                        {
                            ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "up";
                        }
                        if (bulletTimer >= 80) bulletTimer = 0;
                    }

                    if (mode == 4)
                    {
                        if (level.Player.X < Owner.X)
                        {
                            ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "left";
                        }
                        else
                        {
                            ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "right";
                        }
                        if (Math.Abs(Owner.Y - level.Player.Y) < 50f)
                        {
                            Owner.X += vector.X * 20f;
                            Owner.Y += vector.Y * 20f;
                        }
                    }

                    if (mode == 5)
                    {
                        Owner.X += vector.X;
                        Owner.Y += vector.Y;
                        if (level.Player.Y < Owner.Y)
                        {
                            ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "up";
                            Owner.Y -= Math.Min(5f, Owner.Y - level.Player.Y);
                        }
                        else
                        {
                            ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "down";
                            Owner.Y += Math.Min(5f, level.Player.Y - Owner.Y);
                        }
                    }

                    if (mode == 6)
                    {
                        bulletTimer++;
                        if (bulletTimer >= 20)
                        {
                            level.MakeBullet(Owner.X, Owner.Y, (float)Math.Sin(theta) * 10f, -(float)Math.Cos(theta) * 10f);
                            level.MakeBullet(Owner.X, Owner.Y, -(float)Math.Sin(theta) * 10f, -(float)Math.Cos(theta) * 10f);
                            bulletTimer = 0;
                            theta += Math.PI / 8d;
                        }
                        if (theta > Math.PI * 2d) theta -= Math.PI * 2d;
                        if (theta < Math.PI / 4d || theta > Math.PI * 7d / 4d)
                        {
                            ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "up";
                        }
                        else if (theta < Math.PI * 3d / 4d)
                        {
                            ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "right";
                        }
                        else if (theta < Math.PI * 5d / 4d)
                        {
                            ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "down";
                        }
                        else
                        {
                            ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "left";
                        }
                    }
                }
            }

            if (Math.Abs(Owner.Y - level.Player.Y) > 1000)
            {
                Owner.X = -200;
                Owner.Y = 0;
                Owner.Active = false;
                Owner.Visible = false;
            }
        }

        public void FacePlayer()
        {
            float dx = level.Player.X - Owner.X;
            float dy = level.Player.Y - Owner.Y;
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (dx < 0)
                {
                    ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "left";
                }
                else
                {
                    ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "right";
                }
            }
            else
            {
                if (dy < 0)
                {
                    ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "up";
                }
                else
                {
                    ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "down";
                }
            }
        }
    }
}
