using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lichen;
using Lichen.Entities;
using Lichen.Util;
using Microsoft.Xna.Framework;

namespace LifeDeath.Components
{
    class PlayerControlComponent : Lichen.Entities.Component, Lichen.Entities.IUpdateComponent
    {
        Scenes.Level level;
        Vector2 d;

        public PlayerControlComponent(Scenes.Level level)
        {
            this.level = level;
        }

        public void Reset()
        {
            //furthestDistance = 0;
            d.X = 0;
            d.Y = 0;
        }

        public void Update()
        {
            Vector2 vector = new Vector2(0, 0);
            if (GlobalServices.InputManager.Held(Lichen.Input.GameCommand.Up))
            {
                vector.Y = -1;
            }
            if (GlobalServices.InputManager.Held(Lichen.Input.GameCommand.Down))
            {
                vector.Y = 1;
            }
            if (GlobalServices.InputManager.Held(Lichen.Input.GameCommand.Left))
            {
                vector.X = -1;
            }
            if (GlobalServices.InputManager.Held(Lichen.Input.GameCommand.Right))
            {
                vector.X = 1;
            }

            if (vector.X != 0f || vector.Y != 0f) vector.Normalize();

            d.X = (d.X * 5f + vector.X) / 6f;
            d.Y = (d.Y * 5f + vector.Y) / 6f;
            Owner.X += d.X * 6f;
            Owner.Y += d.Y * 6f;

            foreach (Entity actor in Owner.ActorList)
            {
                if (Object.ReferenceEquals(actor, Owner)) continue;
                if (Lichen.Util.MathHelper.Distance(Owner, actor) < 50d)
                {
                    ((Game1)Lichen.GlobalServices.Game).ChangeScene(2);
                    return;
                }
            }

            level.UpdateDistance(Owner.Y);
        }
    }
}
