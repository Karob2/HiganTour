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
    public class SongLibrary : Library<string, Song>
    {
        string fallback = "default:default";
        ContentManager contentManager;

        public SongLibrary()
        {
            contentManager = GlobalServices.NewContentManager();
        }

        protected override Song Load(string path)
        {
            Song song;
            Pathfinder pathfinder = Pathfinder.Find(path, "bgm", Pathfinder.FileType.bgm);
            if (pathfinder.Path == null)
            {
                // Load fallback asset.
                Error.LogError("Failed to find texture <" + path + ">. Loading fallback asset <" + fallback + ">.");
                pathfinder = Pathfinder.Find(fallback, "bgm", Pathfinder.FileType.image);
                if (pathfinder.Path == null)
                {
                    Error.LogErrorAndShutdown("Failed to find fallback asset.");
                }
            }
            if (pathfinder.Ext.Equals("xnb"))
            {
                contentManager.RootDirectory = pathfinder.ContentPath;
                song = contentManager.Load<Song>(pathfinder.ContentFile);
            }
            else if (pathfinder.Ext.Equals("ogg"))
            {
                song = Song.FromUri(pathfinder.Path, new Uri(pathfinder.Path, UriKind.Relative));
            }
            else
            {
                song = null;
                Error.LogErrorAndShutdown("Song file type not supported yet.");
            }
            return song;
        }

        protected override void Unload(Song song)
        {
            song.Dispose();
        }

        public override void Unload()
        {
            contentManager.Unload();
            base.Unload();
        }
    }
}
