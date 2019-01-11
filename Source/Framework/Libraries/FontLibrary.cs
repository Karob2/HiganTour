using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Lichen.Util;

// TODO: Currently each Font object retains its own copy of the specified SpriteFont file.
//   This is wasteful if there are multiple font objects using the same spritefont.
//   Ideally, multiple font objects will not use the same spritefont.

namespace Lichen.Libraries
{
    public class FontLibrary : Library<string, Font>
    {
        string fallback = "default:default";
        string fallback2 = "default:Pixellari_16px";

        ContentManager contentManager;

        public FontLibrary()
        {
            contentManager = GlobalServices.GlobalContent; // For now, I'm considering all fonts global (permanent) content.
        }

        protected override Font Load(string path)
        {
            Pathfinder pathfinder = Pathfinder.Find(path, "fonts", Pathfinder.FileType.json);
            if (pathfinder.Path == null)
            {
                // Load fallback asset.
                Error.LogError("Failed to find font config <" + path + ">. Loading fallback asset <" + fallback + ">.");
                pathfinder = Pathfinder.Find(fallback, "fonts", Pathfinder.FileType.json);
                if (pathfinder.Path == null)
                {
                    Error.LogErrorAndShutdown("Failed to find fallback asset.");
                }
            }
            Font font = JsonHelper<Font>.Load(pathfinder.Path);

            Pathfinder.SetCurrentPath(pathfinder);
            Pathfinder pathfinder2 = Pathfinder.Find(font.FontFile, "fonts", Pathfinder.FileType.xnb);
            if (pathfinder2.Path == null)
            {
                // Load fallback asset.
                Error.LogError("Failed to find font <" + font.FontFile + ">. Loading fallback asset <" + fallback2 + ">.");
                pathfinder2 = Pathfinder.Find(fallback2, "fonts", Pathfinder.FileType.xnb);
                if (pathfinder.Path == null)
                {
                    Error.LogErrorAndShutdown("Failed to find fallback asset.");
                }
            }
            contentManager.RootDirectory = pathfinder2.ContentPath;
            font.SetFont(contentManager.Load<SpriteFont>(pathfinder2.ContentFile));
            Pathfinder.ClearCurrentPath();

            return font;
        }
    }
}
