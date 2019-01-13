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

        int mode;

        public SeekerAIComponent(Scenes.Level level)
        {
            this.level = level;
            target = level.Player;
        }

        public void SetAIMode(int location)
        {
            //mode = location % (Math.Min(location / 4 + 1, 3)) + 1;
            mode = location % 4 + 1;
            bulletTimer = 0;
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
                        Owner.X += vector.X;
                        Owner.Y += vector.Y;
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
