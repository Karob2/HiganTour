using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Lichen.Libraries
{
    [JsonObject]
    public class Font
    {
        [JsonProperty]
        public string FontFile { get; set; }
        [JsonProperty]
        public float Scale { get; set; }

        SpriteFont spriteFont;
        [JsonIgnore]
        public SpriteFont SpriteFont { get { return spriteFont; } set { spriteFont = value; } }

        public Font()
        {

        }

        public void SetFont(SpriteFont spriteFont)
        {
            this.spriteFont = spriteFont;
            this.spriteFont.DefaultCharacter = '?';
        }

        public void Render(string message, float x, float y, Color color, float scale = 1f, float depth = 1f)
        {
            GlobalServices.SpriteBatch.DrawString(spriteFont, message, new Vector2(x, y),
                color, 0f, new Vector2(0f, 0f), Scale * scale, SpriteEffects.None, depth);
        }
    }
}
