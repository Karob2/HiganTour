using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;

namespace HiganTour.Components.SceneComponents
{
    public class LevelUpdateComponent : Lichen.Entities.Component, Lichen.Entities.IUpdateComponent
    {
        Entity target;
        int furthestDistance;
        float distanceTraveled;
        bool enabled = true;

        /*
        public LevelUpdateComponent(Entity target)
        {
            this.target = target;
        }
        */

        public void Update()
        {
            if (!enabled) return;
            UpdateDistance();
        }

        public void UpdateDistance()
        {
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
            if (target == null)
            {
                target = Owner.Scene.GetEntity("camera");
                // If failed to attach to a camera, then stop updating.
                if (target == null)
                {
                    enabled = false;
                    return;
                }
            }

            float delta;
            float y = -target.Y;
            if (y > distanceTraveled)
            {
                delta = y - distanceTraveled;
                distanceTraveled = y;
            }
            else
            {
                delta = 0f;
            }

            int newDistance = (int)(distanceTraveled / 500f);
            if (newDistance > furthestDistance)
            {
                furthestDistance = newDistance;
                MakeEnemy();
                /*
                enemies[currentEnemy].SetPosition(player.X, player.Y - 400f).AttachTo(enemyContainer)
                    .AddActor(actorList);
                currentEnemy++;
                if (currentEnemy >= 20) currentEnemy = 0;
                */
            }

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
            if (Karma < 8f && !debugMode)
            {
                ((Game1)Lichen.GlobalServices.Game).ChangeScene(2);
            }
        }
    }
}
