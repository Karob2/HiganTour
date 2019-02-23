using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lichen
{
    public static class GlobalServices
    {
        // TODO: This class is probably bad design, haphazardly giving static access to many intanced objects.

        public static string GameName { get; set; }
        public static string CompanyName { get; set; }
        public static string ContentDirectory { get; set; }
        public static string SaveDirectory { get; set; }
        public static List<string> ExtensionDirectories { get; set; }

        public static Game Game { get; set; }
        public static GraphicsDeviceManager GraphicsManager { get; set; }
        public static ContentManager GlobalContent { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        //public static GameTime GameTime { get; set; }
        
        static float deltaSeconds;
        static float deltaDrawSeconds;
        public static float DeltaSeconds
        {
            get { return deltaSeconds; }
            // Keep too much "elapsed time" from piling up. Throttles game speed when FPS drops below 30.
            set { deltaSeconds = Math.Min(value, 1f / 30f); }
        }
        public static float DeltaDrawSeconds
        {
            get { return deltaDrawSeconds; }
            // Throttles render and animation speed when FPS drops below 30.
            set { deltaDrawSeconds =  Math.Min(value, 1f / 30f); }
        }
        
        //public static float DeltaSeconds { get; set; }
        //public static float DeltaDrawSeconds { get; set; }
        public static Libraries.TextureLibrary GlobalTextures { get; set; }
        public static Libraries.SpriteLibrary GlobalSprites { get; set; }
        public static Libraries.FontLibrary GlobalFonts { get; set; }
        public static Libraries.SongLibrary GlobalSongs { get; set; }
        public static Libraries.SoundEffectLibrary GlobalSoundEffects { get; set; }

        //public static Entities.EntityProvider GlobalEntityProvider { get; set; }
        public static Dictionary<string, Entities.Entity> EntityLibrary { get; set; }


        public static Input.InputManager InputManager { get; set; }

        public static Input.TextHandler TextHandler { get; set; }

        public static Random GlobalRandom { get; set; }

        public static void Initialize(string gameName, string companyName, Game game, GraphicsDeviceManager graphicsManager)
        {
            GameName = gameName;
            CompanyName = companyName;
            Game = game;
            GraphicsManager = graphicsManager;
            GlobalContent = game.Content;
            SpriteBatch = new SpriteBatch(game.GraphicsDevice);

            ContentDirectory = "Content";
#if DEBUG
            ContentDirectory = Path.Combine("..", "..", "..", "..", "Content");
#endif
            //GlobalContent.RootDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), ContentDirectory);
            //System.Diagnostics.Debug.WriteLine(Path.GetFullPath(ContentDirectory));
            // TODO: How does this perform on linux? (And any other target OS.)
            //SaveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "My Games", gameName);
            if (companyName == null || companyName == "")
            {
                SaveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), gameName);
            }
            else
            {
                SaveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), companyName, gameName);
            }
            // TODO: Set up a way for apps to specify whether to use a global or user-specific save location.
            //SaveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), gameName);
            // Probably don't need to support Roaming data.
            // Ugh, should I wash input and output so that backslashes in custom content don't break linux?
            // Path.DirectorySeparatorChar
            Directory.CreateDirectory(SaveDirectory);
            /*
            string configPath = Path.Combine(SaveDirectory, "config.xml");
            if (!File.Exists(configPath))
                File.Create(configPath).Dispose();
            */

            Util.Error.StartLog();

            string extensionPath = GetSaveDirectory("Extensions");
            Directory.CreateDirectory(extensionPath);
            ExtensionDirectories = new List<string>();
/*
#if DEBUG
            // In debug builds, also check the content folder in the root directory of the project.
            // For the sake of lightweight history, this volatile folder is excluded from the repository.
            ExtensionDirectories.Add(Path.Combine("..", "..", "..", "..", "..", "Content"));
#endif
*/
            // TODO: As a debug, all extensions are automatically loaded. I'm not certain what the final behaviour
            //   should be.
            foreach (string folder in Directory.GetDirectories(extensionPath))
            {
                //ExtensionDirectories.Add(Path.GetFileName(folder));
                ExtensionDirectories.Add(folder);
            }
            // TODO: Make sure extension folder names only include _0-9A-Za-z
/*
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Pathfinder test:");
            System.Diagnostics.Debug.WriteLine(Pathfinder.Find("ball", "sprites", Pathfinder.FileType.image).Path);
            System.Diagnostics.Debug.WriteLine(Pathfinder.Find("ball", "spriteconfigs", Pathfinder.FileType.xml).Path);
            Pathfinder pf = Pathfinder.Find("kraken:/round/ball2", "sprites", Pathfinder.FileType.image);
            System.Diagnostics.Debug.WriteLine(pf.Path);
            System.Diagnostics.Debug.WriteLine(Pathfinder.Find("../rootball", "spriteconfigs", Pathfinder.FileType.xml, pf).Path);
            System.Diagnostics.Debug.WriteLine(Pathfinder.Find("superball", "spriteconfigs", Pathfinder.FileType.xml, pf).Path);
#endif
*/
            GlobalRandom = new Random();

            Libraries.TextureLibrary.Initialize();
            Libraries.SpriteLibrary.Initialize();
            Libraries.FontLibrary.Initialize();
            Libraries.SongLibrary.Initialize();
            Libraries.SoundEffectLibrary.Initialize();

            GlobalTextures = new Libraries.TextureLibrary();
            Libraries.TextureLibrary.AddLibrary(GlobalTextures);

            GlobalSprites = new Libraries.SpriteLibrary(GlobalTextures);
            Libraries.SpriteLibrary.AddLibrary(GlobalSprites);

            GlobalFonts = new Libraries.FontLibrary();
            Libraries.FontLibrary.AddLibrary(GlobalFonts);

            GlobalSongs = new Libraries.SongLibrary();
            Libraries.SongLibrary.AddLibrary(GlobalSongs);

            GlobalSoundEffects = new Libraries.SoundEffectLibrary();
            Libraries.SoundEffectLibrary.AddLibrary(GlobalSoundEffects);

            //GlobalEntityProvider = new Entities.EntityProvider();
            EntityLibrary = new Dictionary<string, Entities.Entity>();

            InputManager = new Input.InputManager(GetSaveDirectory("inputconfig.json"));

            TextHandler = new Input.TextHandler();
        }

        public static void Update(GameTime gameTime)
        {
            DeltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            InputManager.Update();
        }

        public static void PostUpdate()
        {
            //GlobalEntityProvider.ProcessQueue();
        }

        public static void StartDrawing(GameTime gameTime)
        {
            DeltaDrawSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Game.GraphicsDevice.Clear(Color.Black);
            //SpriteBatch.Begin(blendState: BlendState.NonPremultiplied);
            SpriteBatch.Begin();
        }

        public static void StopDrawing()
        {
            SpriteBatch.End();
        }

        public static ContentManager NewContentManager()
        {
            return new ContentManager(GlobalContent.ServiceProvider, GlobalContent.RootDirectory);
        }

        public static string GetSaveDirectory(string filename)
        {
            return Path.Combine(SaveDirectory, filename);
        }

        public static void ExitGame()
        {
            Util.Error.EndLog();
            Game.Exit();
        }
    }
}
