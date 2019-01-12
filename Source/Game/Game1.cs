using Lichen;
using Lichen.Libraries;
using Lichen.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LifeDeath
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphicsManager;
        const string gameName = "thfgj3-lifedeath";
        const string companyName = "";
        Entity root;
        Scenes.Scene mainMenu;
        Scenes.Scene level;
        Scenes.Scene gameOver;


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
            root = new Entity();
            mainMenu = new Scenes.MainMenu();
            mainMenu.Preload(root);
            mainMenu.Load();
            level = new Scenes.Level();
            level.Preload(root);
            level.Load();
            gameOver = new Scenes.GameOver();
            gameOver.Preload(root);
            gameOver.Load();
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

            GlobalServices.Update(gameTime);

            root.Update(); // Update main components.
            root.Update("control"); // Update control components.
            root.Update("motion"); // Update motion components.

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
            switch (sceneNumber)
            {
                case 1:
                    mainMenu.Deactivate();
                    level.Activate();
                    ((Scenes.Level)level).Reset();
                    gameOver.Deactivate();
                    break;
                case 2:
                    mainMenu.Deactivate();
                    level.Deactivate();
                    gameOver.Activate();
                    break;
                default:
                    mainMenu.Activate();
                    level.Deactivate();
                    gameOver.Deactivate();
                    break;
            }
        }
    }
}
