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
    [Flags]
    public enum AIMode
    {
        None,
        SlowWalk,
        ShootThree,
        DashStop,
        HorizontalSlice,
        VerticalFollow,
        ShootRotating,

        Active = 0x1000
    }

    class AIComponent : Component
    {
        public AIMode Mode { get; set; }
        public int Timer { get; set; }
        public float Angle { get; set; }
        public Vector2 Home { get; set; }

        public override void AttachTo(Entity entity)
        {
            entity.AddComponent(this);
        }
    }

    public class AISystem : Lichen.Entities.System
    {
        public override void Update(Scene scene)
        {
            ComponentGroup<AIComponent> aiComponents = scene.GetComponentGroup<AIComponent>();
            ComponentGroup<SpriteComponent> spriteComponents = scene.GetComponentGroup<SpriteComponent>();

            foreach (AIComponent aiComponent in aiComponents.EnabledComponents)
            {
                if (aiComponent.Mode == AIMode.None) continue;
                Entity owner = aiComponent.Owner;

                // Initialize if necessary.
                if ((aiComponent.Mode & AIMode.Active) != AIMode.Active)
                {
                    aiComponent.Mode &= AIMode.Active;
                    // TODO: Maybe use BodyComponent coordinates instead.
                    aiComponent.Home = new Vector2(owner.X, owner.Y);
                    aiComponent.Timer = 0;
                    aiComponent.Angle = 0;
                    if (!spriteComponents.TryGetByOwner(owner, out SpriteComponent spriteComponent))
                    {
                        // TODO: How should I handle this? Create blank sprite? Ignore and move on? Crash?
                        //spriteComponent = null;
                        Error.LogErrorAndShutdown("Player without Sprite.");
                        //continue;
                    }
                    switch (aiComponent.Mode & (~AIMode.Active))
                    {
                        case AIMode.SlowWalk:
                            spriteComponent.SetSprite(GlobalServices.GlobalSprites.Lookup("higantour:fairy_red_sm"));
                            break;
                        case AIMode.ShootThree:
                            spriteComponent.SetSprite(GlobalServices.GlobalSprites.Lookup("higantour:fairy_green_sm"));
                            break;
                        case AIMode.DashStop:
                            spriteComponent.SetSprite(GlobalServices.GlobalSprites.Lookup("higantour:kedama"));
                            break;
                        case AIMode.HorizontalSlice:
                            spriteComponent.SetSprite(GlobalServices.GlobalSprites.Lookup("higantour:scythe"));
                            break;
                        case AIMode.VerticalFollow:
                            spriteComponent.SetSprite(GlobalServices.GlobalSprites.Lookup("higantour:spinner"));
                            break;
                        case AIMode.ShootRotating:
                            spriteComponent.SetSprite(GlobalServices.GlobalSprites.Lookup("higantour:fairy_blue_sm"));
                            break;
                    }
                }
            }
        }
    }
}
