using Lichen;
using Lichen.Libraries;
using Lichen.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HiganTour
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphicsManager;
        const string gameName = "Higan Tour";
        const string companyName = "";
        Entity root;
        Scenes.SceneBase mainMenu;
        Scenes.SceneBase level;
        Scenes.SceneBase gameOver;
        Scenes.SceneBase MusicRoom;

        public bool MR = false;

        public Game1()
        {
            graphicsManager = new GraphicsDeviceManager(this);
        }

        // Called once when the game starts.
        protected override void Initialize()
        {
            graphicsManager.PreferredBackBufferWidth = 1280;
            graphicsManager.PreferredBackBufferHeight = 720;
            graphicsManager.ApplyChanges();

            //graphicsManager.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;

            GlobalServices.Initialize(gameName, companyName, this, graphicsManager);
            base.Initialize();
        }

        // Called once after initialization.
        protected override void LoadContent()
        {
            // Preload music.
            GlobalServices.GlobalSongs.Register("higantour:Main_Menu");
            GlobalServices.GlobalSongs.Register("higantour:Stage");
            GlobalServices.GlobalSongs.Register("higantour:Another_Stage");
            GlobalServices.GlobalSongs.Register("higantour:Boss_Battle");
            GlobalServices.GlobalSongs.Register("higantour:Game_Over");
            
            // Preload all scene-specific assets.
            root = new Entity();
            Scenes.Common.Preload();
            mainMenu = new Scenes.MainMenu();
            mainMenu.Preload(root);
            mainMenu.Load();
            level = new Scenes.Level();
            level.Preload(root);
            level.Load();
            gameOver = new Scenes.GameOver();
            gameOver.Preload(root);
            gameOver.Load();
            MusicRoom = new Scenes.MusicRoom();
            MusicRoom.Preload(root);
            MusicRoom.Load();
            ChangeScene(0);
        }

        // Called when exiting the game.
        protected override void UnloadContent()
        {
            // Unload any non ContentManager content here
        }

        // Called once every frame.
        protected override void Update(GameTime gameTime)
        {
            // Shift+Esc will close the game.
            if ((Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
                && Keyboard.GetState().IsKeyDown(Keys.Escape))
                GlobalServices.ExitGame();

            if (Keyboard.GetState().IsKeyDown(Keys.F11))
            {
                if (!level.IsActive())
                {
                    ChangeScene(1);
                    ((Scenes.Level)level).SetDebugMode();
                }
                else if (!((Scenes.Level)level).DebugMode)
                {
                    ((Scenes.Level)level).SetDebugMode();
                }
            }


            GlobalServices.Update(gameTime);

            root.Update(); // Update main components.
            // TODO: Remove these after switching to the new scene handling system.
            //root.Update("control"); // Update control components.
            //root.Update("motion"); // Update motion components.

            GlobalServices.PostUpdate();

            base.Update(gameTime);
        }

        // Called once every frame. (Or less, if FPS is low.)
        protected override void Draw(GameTime gameTime)
        {
            GlobalServices.StartDrawing(gameTime);
            root.Render();
            GlobalServices.StopDrawing();

            base.Draw(gameTime);
        }

        public void ChangeScene(int sceneNumber)
        {
            ((Scenes.Level)level).PlayerSfxInstance.Stop();

            switch (sceneNumber)
            {
                case 1:
                    mainMenu.Deactivate();
                    level.Activate();
                    ((Scenes.Level)level).Reset();
                    gameOver.Deactivate();
                    MusicRoom.Deactivate();
                    bgm = GlobalServices.GlobalSongs.Lookup("higantour:Stage");
                    MediaPlayer.Volume = 0.5f;
                    MediaPlayer.Play(bgm);
                    MediaPlayer.IsRepeating = true;
                    break;
                case 2:
                    mainMenu.Activate();
                    level.Deactivate();
                    gameOver.Deactivate();
                    MusicRoom.Deactivate();
                    ((Scenes.MainMenu)mainMenu).SetMode(1);
                    bgm = GlobalServices.GlobalSongs.Lookup("higantour:Game_Over");
                    MediaPlayer.Volume = 0.5f;
                    MediaPlayer.Play(bgm);
                    MediaPlayer.IsRepeating = true;
                    break;
                case 3:
                    mainMenu.Deactivate();
                    level.Deactivate();
                    gameOver.Deactivate();
                    MusicRoom.Activate();
                    MR = true;
                    MediaPlayer.Stop();
                    /*
                    bgm = GlobalServices.GlobalSongs.Lookup("higantour:Another_Stage");
                    MediaPlayer.Volume = 0.5f;
                    MediaPlayer.Play(bgm);
                    MediaPlayer.IsRepeating = true;
                    */
                    break;
                default:
                    mainMenu.Activate();
                    level.Deactivate();
                    gameOver.Deactivate();
                    MusicRoom.Deactivate();
                    ((Scenes.MainMenu)mainMenu).SetMode(0);
                    MR = false;
                    bgm = GlobalServices.GlobalSongs.Lookup("higantour:Main_Menu");
                    MediaPlayer.Volume = 0.5f;
                    MediaPlayer.Play(bgm);
                    MediaPlayer.IsRepeating = true;
                    break;
            }
        }

        public bool GetMR() { return MR; }


        public int SN;

        Song bgm;

        public void SwitchSong(bool up)
        {

            if (up)
            {
                SN++;
            } else {
                SN--;
            }

            if (SN > 5) { SN = 0; }
            if (SN < 0) { SN = 5; }

            switch (SN)
            {
                case 0:
                    MRPause();
                    break;

                case 1:
                    ((Scenes.MusicRoom) MusicRoom).Background(1);

                    ((Scenes.MusicRoom)MusicRoom).Menu(1);

                    bgm = GlobalServices.GlobalSongs.Lookup("higantour:Main_Menu");

                    MediaPlayer.Volume = 0.5f;
                    MediaPlayer.Play(bgm);
                    MediaPlayer.IsRepeating = true;
                    break;
                case 2:
                    ((Scenes.MusicRoom)MusicRoom).Background(1);

                    ((Scenes.MusicRoom)MusicRoom).Menu(2);

                    bgm = GlobalServices.GlobalSongs.Lookup("higantour:Stage");

                    MediaPlayer.Volume = 0.5f;
                    MediaPlayer.Play(bgm);
                    MediaPlayer.IsRepeating = true;
                    break;
                case 3:
                    ((Scenes.MusicRoom)MusicRoom).Background(2);

                    ((Scenes.MusicRoom)MusicRoom).Menu(3);

                    bgm = GlobalServices.GlobalSongs.Lookup("higantour:Another_Stage");

                    MediaPlayer.Volume = 0.5f;
                    MediaPlayer.Play(bgm);
                    MediaPlayer.IsRepeating = true;
                    break;
                case 4:
                    ((Scenes.MusicRoom)MusicRoom).Background(3);

                    ((Scenes.MusicRoom)MusicRoom).Menu(4);

                    bgm = GlobalServices.GlobalSongs.Lookup("higantour:Boss_Battle");

                    MediaPlayer.Volume = 0.5f;
                    MediaPlayer.Play(bgm);
                    MediaPlayer.IsRepeating = true;
                    break;

                case 5:
                    ((Scenes.MusicRoom)MusicRoom).Background(1);

                    ((Scenes.MusicRoom)MusicRoom).Menu(5);

                    bgm = GlobalServices.GlobalSongs.Lookup("higantour:Game_Over");

                    MediaPlayer.Volume = 0.5f;
                    MediaPlayer.Play(bgm);
                    MediaPlayer.IsRepeating = true;
                    break;
            }
        }
        public void MRPause() {
            ((Scenes.MusicRoom)MusicRoom).Menu(0);

            ((Scenes.MusicRoom)MusicRoom).Background(0);

            MediaPlayer.Pause();
        }

    }
}
