using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lichen.Util;

namespace Lichen.Libraries
{
    public class SpriteLibrary : Library<string, Sprite>
    {
        string fallback = "default:default";
        TextureLibrary textureLibrary;

        public SpriteLibrary(TextureLibrary textureLibrary)
        {
            this.textureLibrary = textureLibrary;
        }

        protected override Sprite Load(string path)
        {
            // TODO: if spriteconfig does not exist, try to load image with same name as a default basic sprite?

            Pathfinder pathfinder = Pathfinder.Find(path, "sprites", Pathfinder.FileType.json);
            if (pathfinder.Path == null)
            {
                // Load fallback asset.
                Error.LogError("Failed to find sprite config <" + path + ">. Loading fallback asset <" + fallback + ">.");
                pathfinder = Pathfinder.Find(fallback, "sprites", Pathfinder.FileType.json);
                if (pathfinder.Path == null)
                {
                    Error.LogErrorAndShutdown("Failed to find fallback asset.");
                }
            }
            //Sprite sprite = JsonHelper<Sprite>.Load(pathfinder.Path);
            //sprite.Finalize(textureLibrary, pathfinder);
            Sprite sprite = Sprite.NewSprite(pathfinder, textureLibrary);
            /*
            Pathfinder.SetCurrentPath(pathfinder);
            sprite.SetTexture(textureLibrary.Register(sprite.TextureFile));
            Pathfinder.ClearCurrentPath(); // TODO: This develops loose ends too easily.
            */
            return sprite;
        }
    }
}
