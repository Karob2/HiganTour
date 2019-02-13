using Lichen;
using Lichen.Entities;
using Lichen.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiganTour.Scenes
{

    public class MusicRoom : Scene
    {

        Font font;

        Entity camera;
        
        Random random;

        Entity lycoris;
        Entity sprite2;
        Entity sprite3;

        Entity bCContainer1;
        Entity bCContainer2;
        Entity bCContainer3;

        Entity menuDot;

        // Create a reference set of entities and load necessary assets.
        public override void Preload(Entity root)
        {
            this.root = root;

            font = GlobalServices.GlobalFonts.Register("higantour:sans");


            Sprite lycorisSprite = GlobalServices.GlobalSprites.Register("higantour:redlily");

            lycoris = new Entity()
                        .AddRenderComponent(new SpriteComponent(lycorisSprite));

            sprite2 = new Entity()
                        .AddRenderComponent(new SpriteComponent(lycorisSprite));

            sprite3 = new Entity()
                        .AddRenderComponent(new SpriteComponent(lycorisSprite));

        }

        public void Background(int bc)
        {
            switch (bc)
            {
                case 0:
                    bCContainer1.SetVisible(false);
                    bCContainer2.SetVisible(false);
                    bCContainer3.SetVisible(false);
                    break;

                case 1:
                    bCContainer1.SetVisible(true);
                    bCContainer2.SetVisible(false);
                    bCContainer3.SetVisible(false);
                    break;

                case 2:
                    bCContainer1.SetVisible(false);
                    bCContainer2.SetVisible(true);
                    bCContainer3.SetVisible(false);
                    break;

                case 3:
                    bCContainer1.SetVisible(false);
                    bCContainer2.SetVisible(false);
                    bCContainer3.SetVisible(true);
                    break;
            }
        }

        public void Menu(int menu) {

            switch (menu) {

                case 0:

                    menuDot.SetPosition(150, 44);
                    break;
                case 1:

                    menuDot.SetPosition(150, 84);
                    break;
                case 2:

                    menuDot.SetPosition(150, 124);
                    break;
                case 3:

                    menuDot.SetPosition(150, 164);
                    break;
                case 4:

                    menuDot.SetPosition(150, 204);
                    break;
                case 5:

                    menuDot.SetPosition(150, 244);
                    break;
            }

        }

        // Create the scene's entities by cloning reference entities.
        public override void Load()
        {
            container = new Entity()
                .AttachTo(root);

            camera = new Entity()
                .SetRenderByDepth(true)
                .AttachTo(container);

            bCContainer1 = new Entity()
                .AttachTo(camera).SetVisible(false);

            bCContainer2 = new Entity()
                .AttachTo(camera).SetVisible(false);

            bCContainer3 = new Entity()
                .AttachTo(camera).SetVisible(false);

            new Entity(0, 0)
                .AddRenderComponent(new TextComponent(font, "Music Room"))
                .AttachTo(container);

            new Entity(0, 40)
                .AddRenderComponent(new TextComponent(font, "Pause"))
                .AddUpdateComponent(new Components.MenuComponent())
                .AttachTo(container);
            new Entity(0, 80)
               .AddRenderComponent(new TextComponent(font, "Main Menu Theme"))
               .AddUpdateComponent(new Components.MenuComponent())
               .AttachTo(container);

            new Entity(0, 120)
                .AddRenderComponent(new TextComponent(font, "Stage 1 Theme"))
                .AddUpdateComponent(new Components.MenuComponent())
                .AttachTo(container);
            new Entity(0, 160)
               .AddRenderComponent(new TextComponent(font, "Stage 2 Theme"))
               .AddUpdateComponent(new Components.MenuComponent())
               .AttachTo(container);

            new Entity(0, 200)
                .AddRenderComponent(new TextComponent(font, "Boss Theme"))
                .AddUpdateComponent(new Components.MenuComponent())
                .AttachTo(container);
            new Entity(0, 240)
               .AddRenderComponent(new TextComponent(font, "Game Over Theme"))
               .AddUpdateComponent(new Components.MenuComponent())
               .AttachTo(container);

           menuDot = new Entity(150, 40)
               .AddRenderComponent(new TextComponent(font, "*"))
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
                    .AttachTo(bCContainer1);
                theta += phi;
                if (theta > 1d) theta -= 1d;
            }

            for (int i = 0; i < 200; i++)
            {
                sprite2.Clone()
                    //.SetPosition(random.Next(0, 700), random.Next(0, 700))
                    //.AddChainComponent("motion", new Components.WindyComponent(random.Next(0, 1280), random.Next(0, 720)))
                    .AddChainComponent("motion", new Components.WindyComponent(null, camera, (float)(theta * 1280d + random.NextDouble() * 200d - 100d), (float)i * 920f / 200f))
                    .AttachTo(bCContainer2);
                theta += phi;
                if (theta > 1d) theta -= 1d;
            }

            for (int i = 0; i < 200; i++)
            {
                sprite3.Clone()
                    //.SetPosition(random.Next(0, 700), random.Next(0, 700))
                    //.AddChainComponent("motion", new Components.WindyComponent(random.Next(0, 1280), random.Next(0, 720)))
                    .AddChainComponent("motion", new Components.WindyComponent(null, camera, (float)(theta * 1280d + random.NextDouble() * 200d - 100d), (float)i * 920f / 200f))
                    .AttachTo(bCContainer3);
                theta += phi;
                if (theta > 1d) theta -= 1d;
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
