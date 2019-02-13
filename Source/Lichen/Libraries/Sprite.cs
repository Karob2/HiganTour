using Lichen.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Lichen.Libraries
{
    public class Sprite
    {
        // TODO: I don't actually need to copy DefaultFrame and DefaultAnimation from _Sprite?
        public Frame DefaultFrame { get; set; }
        public string DefaultAnimation { get; set; }
        public Dictionary<string, Animation> Animations { get; set; }

        public static Sprite NewSprite(Pathfinder path, TextureLibrary textureLibrary)
        {
            _Sprite _sprite = JsonHelper<_Sprite>.Load(path.Path);
            _sprite.Finalize(textureLibrary, path);
            Sprite sprite = _sprite.Solidify();
            return sprite;
        }

        public void Render(float x, float y, string currentAnimation, int currentFrame)
        {
            // TODO: Throw error if CurrentAnimation does not exist in Animations?
            Animation animation = Animations[currentAnimation];
            Frame frame = animation.Frames[currentFrame];
            GlobalServices.SpriteBatch.Draw(
                DefaultFrame.Texture,
                new Vector2(x - frame.AnchorX, y - frame.AnchorY),
                new Rectangle(frame.X, frame.Y, frame.Width, frame.Height),
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
                );
        }
    }

    public class Frame
    {
        // TODO: String 'Spritesheet' may not be needed if texture is always loaded already.
        public string Spritesheet { get; set; } = null;
        public int X { get; set; } = -1;
        public int Y { get; set; } = -1;
        public int Width { get; set; } = -1;
        public int Height { get; set; } = -1;
        public float AnchorX { get; set; } = -2e38f;
        public float AnchorY { get; set; } = -2e38f;
        public float Frametime { get; set; } = 1f;
        //public int Count { get; set; } = -1;

        public Texture2D Texture { get; set; }

        public Frame Clone()
        {
            return (Frame)this.MemberwiseClone();
        }
    }

    public class Animation
    {
        public string Name { get; set; }
        public float Speed { get; set; } = 1f;
        public List<Frame> Frames { get; set; }
    }

    [JsonObject]
    public class _Sprite
    {
        [JsonProperty]
        public _FrameRange DefaultFrame { get; set; }
        [JsonProperty]
        public string DefaultAnimation { get; set; }
        [JsonProperty]
        public List<_Animation> Animations { get; set; }

        public void Finalize(TextureLibrary textureLibrary, Pathfinder defaultPath)
        {
            Pathfinder.SetCurrentPath(defaultPath);

            if (DefaultFrame == null) DefaultFrame = new _FrameRange();
            if (DefaultFrame.Spritesheet != null)
            {
                DefaultFrame.Texture = textureLibrary.Register(DefaultFrame.Spritesheet);
            }
            if (DefaultFrame.X == null) DefaultFrame.X = 0;
            if (DefaultFrame.Y == null) DefaultFrame.Y = 0;
            if (DefaultFrame.Texture != null)
            {
                if (DefaultFrame.Width == null) DefaultFrame.Width = DefaultFrame.Texture.Width;
                if (DefaultFrame.Height == null) DefaultFrame.Height = DefaultFrame.Texture.Height;
            }
            else
            {
                DefaultFrame.Width = 0;
                DefaultFrame.Height = 0;
            }
            if (DefaultFrame.AnchorX == null) DefaultFrame.AnchorX = 0f;
            if (DefaultFrame.AnchorY == null) DefaultFrame.AnchorY = 0f;
            if (DefaultFrame.Count == null) DefaultFrame.Count = 1;
            if (DefaultFrame.Frametime == null) DefaultFrame.Frametime = 1f;

            if (Animations == null)
            {
                Animations = new List<_Animation>(new _Animation[] { new _Animation() });
            }
            foreach (_Animation animation in Animations)
            {
                if (animation.Name == null) animation.Name = "default";
                if (animation.Speed == null) animation.Speed = 1f;
                if (animation.FrameRanges == null)
                {
                    animation.FrameRanges = new List<_FrameRange>();
                    animation.FrameRanges.Add(DefaultFrame);
                }
                else
                {
                    foreach (_FrameRange frame in animation.FrameRanges)
                    {
                        if (frame.Spritesheet == null)
                        {
                            frame.Spritesheet = DefaultFrame.Spritesheet;
                            frame.Texture = DefaultFrame.Texture;
                        }
                        else
                        {
                            frame.Texture = textureLibrary.Register(frame.Spritesheet);
                        }
                        if (frame.X == null) frame.X = DefaultFrame.X;
                        if (frame.Y == null) frame.Y = DefaultFrame.Y;
                        if (frame.Width == null) frame.Width = DefaultFrame.Width;
                        if (frame.Height == null) frame.Height = DefaultFrame.Height;
                        if (frame.AnchorX == null) frame.AnchorX = DefaultFrame.AnchorX;
                        if (frame.AnchorY == null) frame.AnchorY = DefaultFrame.AnchorY;
                        if (frame.Count == null) frame.Count = DefaultFrame.Count;
                        if (frame.Frametime == null) frame.Frametime = 1f; // Do not inherit this from DefaultFrame.
                    }
                }

                if (DefaultAnimation == null) DefaultAnimation = animation.Name;
            }

            Pathfinder.ClearCurrentPath(); // TODO: This develops loose ends too easily.
        }

        public Sprite Solidify()
        {
            Sprite sprite = new Sprite();
            sprite.DefaultFrame = DefaultFrame.Solidify().First();
            sprite.DefaultAnimation = DefaultAnimation;

            sprite.Animations = new Dictionary<string, Animation>();
            foreach (_Animation _animation in Animations)
            {
                sprite.Animations.Add(_animation.Name, _animation.Solidify());
            }

            return sprite;
        }
    }

    [JsonObject]
    public class _FrameRange
    {
        [JsonProperty]
        public string Spritesheet { get; set; }
        [JsonProperty]
        public int? X { get; set; }
        [JsonProperty]
        public int? Y { get; set; }
        [JsonProperty]
        public int? Width { get; set; }
        [JsonProperty]
        public int? Height { get; set; }
        [JsonProperty]
        public float? AnchorX { get; set; }
        [JsonProperty]
        public float? AnchorY { get; set; }
        [JsonProperty]
        public float? Frametime { get; set; }
        [JsonProperty]
        public int? Count { get; set; }

        [JsonIgnore]
        public Texture2D Texture { get; set; }

        public List<Frame> Solidify()
        {
            Frame frame = new Frame();
            frame.Spritesheet = Spritesheet;
            frame.X = X.Value;
            frame.Y = Y.Value;
            frame.Width = Width.Value;
            frame.Height = Height.Value;
            frame.AnchorX = AnchorX.Value;
            frame.AnchorY = AnchorY.Value;
            frame.Frametime = Frametime.Value;
            frame.Texture = Texture;

            List<Frame> frames = new List<Frame>();
            frames.Add(frame);
            for (int i = 1; i < Count.Value; i++)
            {
                frame = frame.Clone();
                frame.X += frame.Width;
                frames.Add(frame);
            }

            return frames;
        }
    }

    [JsonObject]
    public class _Animation
    {
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public float? Speed { get; set; }
        [JsonProperty]
        public List<_FrameRange> FrameRanges { get; set; }

        public Animation Solidify()
        {
            Animation animation = new Animation();
            animation.Name = Name;
            animation.Speed = Speed.Value;
            if (FrameRanges != null)
            {
                animation.Frames = new List<Frame>();
                foreach (_FrameRange _frameRange in FrameRanges)
                {
                    List<Frame> frames = _frameRange.Solidify();
                    foreach (Frame frame in frames)
                    {
                        animation.Frames.Add(frame);
                    }
                }
            }
            return animation;
        }
    }
}
