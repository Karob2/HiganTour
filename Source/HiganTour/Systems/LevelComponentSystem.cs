using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;
using HiganTour.Components;
using Microsoft.Xna.Framework;
using Lichen;
using Lichen.Entities.Components;
using Lichen.Util;

namespace HiganTour.Systems
{
    class LevelComponent : Component
    {
        public float FurthestDistance { get; set; }

        public override void AttachTo(Entity entity)
        {
            entity.AddComponent(this);
        }
    }

    class LevelSystem : Lichen.Entities.System
    {
        Random random = new Random();

        public override void Update(Scene scene)
        {
            ComponentGroup<LevelComponent> levelComponents = scene.GetComponentGroup<LevelComponent>();
            ComponentGroup<BodyComponent> bodyComponents = scene.GetComponentGroup<BodyComponent>();

            foreach (LevelComponent levelComponent in levelComponents.EnabledComponents)
            {
                Entity player = scene.GetEntity("player");
                if (player == null) return;
                Entity camera = scene.GetEntity("camera");
                if (camera == null) return;

                /*
                if (delta < 0)
                {
                    distanceTraveled -= delta;
                }
                */
                /*
                else if (delta > 0)
                {
                    Karma -= 0.5f;
                    KarmaChanged = true;
                }
                */
                float distanceTraveled = -player.Y;

                int newDistance = (int)(distanceTraveled / 500f);
                if (newDistance > levelComponent.FurthestDistance)
                {
                    levelComponent.FurthestDistance = newDistance;

                    //start MakeEnemy(player, camera, levelComponent);
                    float x = (float)random.NextDouble() * 1180f + 50f;
                    float y = player.Y - 700f;

                    int mode = (int)levelComponent.FurthestDistance % (Math.Min((int)levelComponent.FurthestDistance / 4 + 1, 6)) + 1;
                    //mode = furthestDistance % 6 + 1;
                    //mode = 4;

                    if (mode == 4)
                    {
                        if (x < 640f) x = 50f;
                        else x = 1230f;
                    }
                    Entity enemy = scene.EntityLibrary["enemy"].CloneTo(camera);
                    if (!bodyComponents.TryGetByOwner(enemy, out BodyComponent bodyComponent))
                    {
                        bodyComponent = new BodyComponent();
                        enemy.AddComponent(bodyComponent);
                    }
                    enemy.SetPosition(x, y);
                    bodyComponent.Position = new Vector3(x, y, 0);

                    //set seeker ai component mode

                    /*
                    warning.X = x;
                    warningTimer = 0;
                    warning.Visible = true;

                    int ninjaCount = (furthestDistance - 20) / 10;
                    for (int i = 0; i < ninjaCount; i++)
                    {
                        x = (float)random.NextDouble() * 1180f + 50f;
                        enemy = enemies[currentEnemy];
                        enemy.SetPosition(x, y);
                        ((Components.AI.SeekerAIComponent)enemy.UpdateChains["control"].First()).SetAIMode(1);
                        enemy.Visible = true;
                        enemy.Active = true;
                        currentEnemy++;
                        if (currentEnemy >= 20) currentEnemy = 0;
                    }
                    */
                    //end MakeEnemy()

                    /*
                    enemies[currentEnemy].SetPosition(player.X, player.Y - 400f).AttachTo(enemyContainer)
                        .AddActor(actorList);
                    currentEnemy++;
                    if (currentEnemy >= 20) currentEnemy = 0;
                    */
                }

                /*
                warningTimer++;
                if (warningTimer > 30) warning.Visible = false;

                if (Hiding > 0)
                {
                    Karma -= 0.5f;
                    KarmaChanged = true;
                }
                if (!KarmaChanged && Karma < 100f) Karma += 0.5f;
                death.X = player.X;
                death.Y = player.Y - Karma * 8f;
                KarmaChanged = false;
                if (Karma < 8f && !DebugMode)
                {
                    ((Game1)Lichen.GlobalServices.Game).ChangeScene(2);
                }
                */

                break; // Only let one level component be active in a scene.
            }
        }

        public void MakeEnemy(Entity player, Entity camera, LevelComponent levelComponent)
        {
        }
    }
}
