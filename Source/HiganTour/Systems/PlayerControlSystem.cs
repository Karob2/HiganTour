using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;
using HiganTour.Components;
using Microsoft.Xna.Framework;
using Lichen;
using Lichen.Entities.Components;

namespace HiganTour.Systems
{
    public class PlayerControlSystem : Lichen.Entities.System
    {
        public override void Update(Scene scene)
        {
            ComponentGroup<PlayerControlComponent> components = scene.GetComponentGroup<PlayerControlComponent>();

            foreach (PlayerControlComponent component in components.List)
            {
                Entity player = component.Owner;

                // TODO: Do I really want to repeat these every frame?
                if (!player.TryGetComponent(out BodyComponent bodyComponent))
                {
                    bodyComponent = new BodyComponent();
                    player.AddComponent(bodyComponent);
                    bodyComponent.Position = new Vector3(player.X, player.Y, 0);
                }
                if (!player.TryGetComponent(out SpriteComponent spriteComponent))
                {
                    spriteComponent = null;
                }

                if (component.DodgeTimer > 0) component.DodgeTimer--;
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

                if (GlobalServices.InputManager.Held(Lichen.Input.GameCommand.Action2))
                {
                    // TODO: What /is/ a good way to change sprite/animation? Give entities SetAction/RunAction delegates by string (similar to SetInt etc)?
                    spriteComponent.CurrentAnimation = "hiding";
                    vector.X = 0;
                    vector.Y = 0;
                    component.Hiding++;
                }
                else
                {
                    spriteComponent.CurrentAnimation = "default";
                    component.Hiding = 0;
                    if (vector.Y > 0)
                    {
                        component.Karma -= 0.5f;
                        //karmaChanged = true;
                    }
                }
                if (component.Hiding > 0) player.AddTag("hiding");
                else player.RemoveTag("hiding");

                if (vector.X != 0f || vector.Y != 0f)
                {
                    vector.Normalize();
                    vector *= 6f;
                    /*
                    rustle.Volume = 0.4f;
                    rustle.IsLooped = true;
                    rustle.Resume();
                    */

                    if (component.DodgeTimer <= 0 && GlobalServices.InputManager.Held(Lichen.Input.GameCommand.Action1))
                    {
                        //dodgeTimer = 60 * 1;
                        component.DodgeTimer = 30;
                        bodyComponent.Velocity = new Vector3(vector * 8f, 0);
                    }
                    else
                    {
                        //bodyComponent.Velocity += new Vector3(vector, 0);
                    }
                }
                else
                {
                    //level.PlayerSfxInstance.Pause();
                    /*
                    rustle.Volume = 0.1f;
                    rustle.IsLooped = true;
                    rustle.Resume();
                    */
                }

                Vector3 move = new Vector3();
                move.X = (bodyComponent.Velocity.X * 5f + vector.X) / 6f;
                move.Y = (bodyComponent.Velocity.Y * 5f + vector.Y) / 6f;
                bodyComponent.Velocity = move;
                /*
                d.X = (d.X * 5f + vector.X) / 6f;
                d.Y = (d.Y * 5f + vector.Y) / 6f;
                Owner.X += d.X * 6f;
                Owner.Y += d.Y * 6f;

                if (Owner.X < 20f)
                {
                    Owner.X = 20f;
                    if (d.X < 0f) d.X = 0f;
                }
                if (Owner.X > 1280f - 20f)
                {
                    Owner.X = 1280f - 20f;
                    if (d.X > 0f) d.X = 0f;
                }
                */
            }
        }
    }
}
