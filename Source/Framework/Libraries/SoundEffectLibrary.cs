using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Lichen.Util;

namespace Lichen.Libraries
{
    public class SoundEffectLibrary : Library<string, SoundEffect>
    {
        string fallback = "default:default";
        ContentManager contentManager;

        public SoundEffectLibrary()
        {
            contentManager = GlobalServices.NewContentManager();
        }

        protected override SoundEffect Load(string path)
        {
            SoundEffect fx;
            Pathfinder pathfinder = Pathfinder.Find(path, "sfx", Pathfinder.FileType.bgm);
            if (pathfinder.Path == null)
            {
                // Load fallback asset.
                Error.LogError("Failed to find texture <" + path + ">. Loading fallback asset <" + fallback + ">.");
                pathfinder = Pathfinder.Find(fallback, "sfx", Pathfinder.FileType.image);
                if (pathfinder.Path == null)
                {
                    Error.LogErrorAndShutdown("Failed to find fallback asset.");
                }
            }
            if (pathfinder.Ext.Equals("xnb"))
            {
                contentManager.RootDirectory = pathfinder.ContentPath;
                fx = contentManager.Load<SoundEffect>(pathfinder.ContentFile);
            }
            else
            {
                fx = null;
                Error.LogErrorAndShutdown("Import type not supported yet.");
            }
            return fx;
        }

        protected override void Unload(SoundEffect fx)
        {
            fx.Dispose();
        }

        public override void Unload()
        {
            contentManager.Unload();
            base.Unload();
        }
    }
}
