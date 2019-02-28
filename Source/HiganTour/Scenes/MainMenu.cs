using Lichen;
using Lichen.Entities;
using Lichen.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiganTour.Components;

namespace HiganTour.Scenes
{
    public class MainMenu : SceneBase
    {
        Entity camera;
        //Entity lycoris;
        Entity title, gameover;
        Font font;
        Random random;

        // Create a reference set of entities and load necessary assets.
        public override void Preload(Scene scene)
        {
            this.Scene = scene; // TODO: Is there a better way?

            font = GlobalServices.GlobalFonts.Register("higantour:sans");

            /*
            Sprite lycorisSprite = GlobalServices.GlobalSprites.Register("higantour:redlily");
            lycoris = new Entity()
                .AddRenderComponent(new SpriteComponent(lycorisSprite));
                */
        }

        // Create the scene's entities by cloning reference entities.
        public override void Load()
        {
            Entity root = Scene.Root;

            //Scene.AddFilter(typeof(RenderComponent));
            Scene.AddUpdateChain("menu");
            Scene.AddUpdateChain("motion");
            Scene.AddSystem(new Systems.WindySystem(), "motion");
            Scene.AddSystem(new Systems.BodySystem(), "motion");
            Scene.AddRenderChain("render");
            Scene.AddSystem(new RenderSystem().AddSubsystem(new Systems.RenderOffsetSubsystem()), "render");

            root.AddComponent(new Components.MenuComponent()); // This component handles the key input.

            camera = root.MakeChild();

            title = camera.MakeChild()
                .SetPosition(640, 250)
                .SetRenderLayer(-1)
                .AddComponent(new BodyComponent())
                .AddComponent(new SpriteComponent(GlobalServices.GlobalSprites.Register("higantour:title")));

            gameover = camera.MakeChild()
                .SetPosition(640, 250)
                .SetRenderLayer(-1)
                .AddComponent(new BodyComponent())
                .AddComponent(new SpriteComponent(GlobalServices.GlobalSprites.Register("higantour:gameover")))
                .SetVisible(false);

            root.MakeChild()
                .SetPosition(20, 20)
                .AddComponent(new TextComponent(font, "v0.3-pre"));

            root.MakeChild()
                .SetPosition(20, 60)
                .AddComponent(new TextComponent(font, "Press Enter to Begin"));

            root.MakeChild()
                .SetPosition(20, 100)
               .AddComponent(new TextComponent(font, "Press M to Enter the Music Room"));

            root.MakeChild()
                .SetPosition(20, 140)
               .AddComponent(new TextComponent(font, "Press F11 to enter Debug Mode"));

            random = new Random(0);
            double phi = (Math.Sqrt(5d) - 1d) / 2d;
            double theta = random.NextDouble();
            for (int i = 0; i < 200; i++)
            {
                //float x = random.Next(0, 1280);
                //float y = random.Next(0, 720);
                float x = (float)(theta * 1280d + random.NextDouble() * 200d - 100d);
                float y = i * 920f / 200f;
                Scenes.Common.Lycoris.CloneTo(camera).SetPosition(x, y);
                /*
                lycoris = camera.MakeChild()
                    //.SetRenderOrder(-1, y)
                    .SetRenderLayer(-1)
                    .AddComponent(new BodyComponent())
                    .AddComponent(new SpriteComponent(lycorisSprite))
                    .SetPosition(x, y)
                    .AddComponent(new Components.WindyComponent())
                    .SetPosition(x, y);
                    //.AddChainComponent("motion", new Components.WindyComponent(random.Next(0, 1280), random.Next(0, 720)))
                    //.AddChainComponent("motion", new Components.WindyComponent(null, camera, (float)(theta * 1280d + random.NextDouble() * 200d - 100d), (float)i * 920f / 200f))
                    */
                theta += phi;
                if (theta > 1d) theta -= 1d;
            }
            //Entity lycorisField = GlobalServices.EntityLibrary["lycoris-field"].Clone();
            //lycorisField.AttachTo(camera);
        }

        public void SetMode(int mode)
        {
            if (mode == 0)
            {
                title.Visible = true;
                gameover.Visible = false;
            }
            if (mode == 1)
            {
                title.Visible = false;
                gameover.Visible = true;
            }
        }

        // Delete the scene. (Reference entities and assets remain.)
        public override void Unload()
        {
            // TODO: Should this also undo the preloading?
            sceneContainer = null;
            // TODO: Is that enough to destroy the scene entities? Or do I need to parse through them all?
            //     Is garbage collection hindered by parent and child referencing each other?
        }
    }
}
