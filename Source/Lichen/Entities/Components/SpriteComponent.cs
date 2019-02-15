using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Entities
{
    public class SpriteComponent : Component, IRenderComponent
    {
        public Libraries.Sprite Sprite { get; set; }

        public string CurrentAnimation { get; set; }
        public int CurrentFrame { get; set; }
        public float CurrentTime { get; set; }

        public SpriteComponent(Libraries.Sprite sprite)
        {
            SetSprite(sprite);
        }

        public void SetSprite(Libraries.Sprite sprite)
        {
            Sprite = sprite;
            CurrentAnimation = sprite.DefaultAnimation;
            CurrentFrame = 0;
            CurrentTime = 0f;
        }

        public void SetAnimation(string animation)
        {
            CurrentAnimation = animation;
            CurrentFrame = 0;
            CurrentTime = 0f;
        }

        public void Render()
        {
            Sprite.Render(Owner.RelativeX, Owner.RelativeY, CurrentAnimation, CurrentFrame);
            // TODO: Updating the animation here makes animations stall when invisible, and can make them less precise when Update() happens more often than Render(), which is significant when events are triggered by animation state. Consider adding capability of animation progress to be Update()-driven.
            // TODO: Throw error if CurrentAnimation does not exist in Animations?
            Libraries.Animation animation = Sprite.Animations[CurrentAnimation];
            CurrentTime += GlobalServices.DeltaDrawSeconds * animation.Speed;
            // TODO: Add option for each animation to be looping or not.
            if (CurrentTime >= animation.Frames[CurrentFrame].Frametime)
            {
                CurrentTime = 0; // TODO: Should be CurrentTime -= animations.Frames[CurrentFrame].Frametime
                CurrentFrame++;
                if (CurrentFrame >= animation.Frames.Count) CurrentFrame = 0; // TODO: Does not account for when multiple frames should pass.
            }
        }
    }
}

/*
namespace Lichen.ExtensionMethods
{
    public static class SpriteComponentExtensionMethods
    {
        public static Entities.Entity AddSprite(this Entities.Entity entity, Libraries.Sprite sprite)
        {
            entity.RenderComponent = new Entities.SpriteComponent(sprite);
            return entity;
        }
    }
}
*/
