﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lichen;
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
        string color = "red_";

        float spawnX, spawnY;

        public SeekerAIComponent(Scenes.Level level)
        {
            this.level = level;
            target = level.Player;
        }

        public void SetAIMode(int mode)
        {
            //mode = location % (Math.Min(location / 4 + 1, 6)) + 1;
            //mode = location % 6 + 1;
            //mode = 4;
            this.mode = mode;
            bulletTimer = 0;
            theta = 0d;
            spawnX = Owner.X;
            spawnY = Owner.Y;

            ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).SetSprite(GlobalServices.GlobalSprites.Lookup("higantour:fairy_sm"));
            if (mode == 1)
            {
                color = "red_";
                ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "down";
            }
            if (mode == 2)
            {
                color = "green_";
                ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "down";
            }
            if (mode == 3)
            {
                //color = "blue_";
                ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).SetSprite(GlobalServices.GlobalSprites.Lookup("higantour:enemy"));
                ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "kedama_idle";
            }
            if (mode == 4)
            {
                //color = "black_";
                //Owner.X = 640f;
                ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).SetSprite(GlobalServices.GlobalSprites.Lookup("higantour:enemy"));
                ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).SetAnimation("scythe_left");
            }
            if (mode == 5)
            {
                ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).SetSprite(GlobalServices.GlobalSprites.Lookup("higantour:enemy"));
                ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).SetAnimation("spinner");
            }
            if (mode == 6)
            {
                color = "blue_";
                ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "down";
            }
        }

        public void Update()
        {
            if (Math.Abs(Owner.Y - level.Player.Y) < 600)
            {
                float focusX = target.X;
                float focusY = target.Y;
                float slowdown = 1f;
                if (level.Hiding <= 20)
                {
                    slowdown = (20f - level.Hiding) / 20f;
                }
                if (level.Hiding > 20)
                {
                    focusX = spawnX;
                    focusY = spawnY;
                }

                Vector2 vector = new Vector2(focusX - Owner.X, focusY - Owner.Y);
                if (vector.Length() < 5f)
                {
                    slowdown = 0f;
                }
                //if (vector.X != 0 || vector.Y != 0)
                else
                {
                    vector.Normalize();
                }

                // Slowly walks toward player.
                if (mode == 1)
                {
                    FaceTarget(focusX, focusY);
                    Owner.X += vector.X * 2f * slowdown;
                    Owner.Y += vector.Y * 2f * slowdown;
                }

                // Stands still and shoot volleys of 3 bullets toward player.
                if (mode == 2 && (level.Hiding == 0 || bulletTimer >= 80))
                {
                    FaceTarget(focusX, focusY);
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

                // Occasionally dashes toward player.
                if (mode == 3)
                {
                    if (level.Hiding > 20)
                    {
                        if (bulletTimer > 40) bulletTimer = 40;
                        ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "kedama_idle";
                    }
                    else
                    {
                        bulletTimer++;
                        if (bulletTimer >= 60)
                        {
                            Owner.X += vector.X * 10f * slowdown;
                            Owner.Y += vector.Y * 10f * slowdown;
                            //FacePlayer();
                            ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "kedama_active";
                        }
                        else
                        {
                            //FacePlayer();
                            //((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "up";
                            ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "kedama_idle";
                        }
                        if (bulletTimer >= 80) bulletTimer = 0;
                    }
                }

                // Madly dashes horizontally toward the player.
                if (mode == 4)
                {
                    if (focusX < Owner.X)
                    {
                        //((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "left";
                        ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "scythe_left";
                    }
                    else
                    {
                        //((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "right";
                        ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = "scythe_right";
                    }
                    if (Math.Abs(Owner.Y - focusY) < 200f)
                    {
                        /*
                        float lag = 0f;
                        if (Owner.X < focusX - 5f) lag = -5f;
                        else if (Owner.X > focusX + 5f) lag = 5f;
                        else slowdown = 0f;
                        Owner.X += Math.Max(Math.Min(focusX - Owner.X + lag, 20f * slowdown), -20f * slowdown);
                        */

                        if (Owner.X < focusX - 5f)
                        {
                            float dist = (focusX - 5f) - Owner.X;
                            float speed = Math.Max(20f * Math.Min(dist, 100f) / 100f, 0f) * slowdown;
                            Owner.X += speed;
                        }
                        else if (Owner.X > focusX + 5f)
                        {
                            float dist = Owner.X - (focusX + 5f);
                            float speed = Math.Max(20f * Math.Min(dist, 100f) / 100f, 0f) * slowdown;
                            Owner.X -= speed;
                        }

                        //Owner.X += vector.X * 20f * slowdown;
                        //Owner.Y += vector.Y * 20f;
                    }
                }

                // Moves vertically to try to trap the player.
                if (mode == 5 && level.Hiding <= 20)
                {
                    Owner.X += vector.X * slowdown;
                    Owner.Y += vector.Y * slowdown;
                    if (focusY < Owner.Y)
                    {
                        //((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "up";
                        Owner.Y -= Math.Min(5f, Owner.Y - focusY) * slowdown;
                    }
                    else
                    {
                        //((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "down";
                        Owner.Y += Math.Min(5f, focusY - Owner.Y) * slowdown;
                    }
                }

                // Shoots bullets in a double rotating pattern.
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
                        ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "up";
                    }
                    else if (theta < Math.PI * 3d / 4d)
                    {
                        ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "right";
                    }
                    else if (theta < Math.PI * 5d / 4d)
                    {
                        ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "down";
                    }
                    else
                    {
                        ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "left";
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

        public void FaceTarget(float targetX, float targetY)
        {
            float dx = targetX - Owner.X;
            float dy = targetY - Owner.Y;
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (dx < 0)
                {
                    ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "left";
                }
                else
                {
                    ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "right";
                }
            }
            else
            {
                if (dy < 0)
                {
                    ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "up";
                }
                else
                {
                    ((Lichen.Entities.SpriteComponent)Owner.RenderComponent).CurrentAnimation = color + "down";
                }
            }
        }
    }
}
