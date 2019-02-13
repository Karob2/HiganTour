using Lichen;
using Lichen.Entities;
using Lichen.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiganTour.Scenes
{
    public class MainMenu : Scene
    {
        Entity camera;
        Entity lycoris;
        Entity title, gameover;
        Font font;
        Random random;

        // Create a reference set of entities and load necessary assets.
        public override void Preload(Entity root)
        {
            this.root = root;

            font = GlobalServices.GlobalFonts.Register("higantour:sans");

            Sprite lycorisSprite = GlobalServices.GlobalSprites.Register("higantour:redlily");
            lycoris = new Entity()
                .AddRenderComponent(new SpriteComponent(lycorisSprite));
        }

        // Create the scene's entities by cloning reference entities.
        public override void Load()
        {
            container = new Entity();
            container.AttachTo(root);
            camera = new Entity()
                .SetRenderByDepth(true)
                .AttachTo(container);

            title = new Entity(640, 250)
                .AddRenderComponent(new SpriteComponent(GlobalServices.GlobalSprites.Register("higantour:title")))
                .AttachTo(camera);

            gameover = new Entity(640, 250)
                .AddRenderComponent(new SpriteComponent(GlobalServices.GlobalSprites.Register("higantour:gameover")))
                .AttachTo(camera).SetVisible(false);

            new Entity(0, 40)
                .AddRenderComponent(new TextComponent(font, "Press Enter to Begin"))
                .AddUpdateComponent(new Components.MenuComponent())
                .AttachTo(container);

            new Entity(0, 80)
               .AddRenderComponent(new TextComponent(font, "Press M to Enter the Music Room"))
               .AddUpdateComponent(new Components.MenuComponent())
               .AttachTo(container);

            random = new Random();
            double phi = (Math.Sqrt(5d) - 1d) / 2d;
            double theta = random.NextDouble();
            for (int i = 0; i < 200; i++)
            {
                lycoris.Clone()
                    //.SetPosition(random.Next(0, 700), random.Next(0, 700))
                    //.AddChainComponent("motion", new Components.WindyComponent(random.Next(0, 1280), random.Next(0, 720)))
                    .AddChainComponent("motion", new Components.WindyComponent(null, camera, (float)(theta * 1280d + random.NextDouble() * 200d - 100d), (float)i * 920f / 200f))
                    .AttachTo(camera);
                theta += phi;
                if (theta > 1d) theta -= 1d;
            }
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
            container = null;
            // TODO: Is that enough to destroy the scene entities? Or do I need to parse through them all?
            //     Is garbage collection hindered by parent and child referencing each other?
        }
    }
}
