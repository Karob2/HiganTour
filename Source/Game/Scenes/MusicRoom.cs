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

namespace LifeDeath.Scenes
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

        // Create a reference set of entities and load necessary assets.
        public override void Preload(Entity root)
        {
            this.root = root;

            font = GlobalServices.GlobalFonts.Register("lifedeath:sans");


            Sprite lycorisSprite = GlobalServices.GlobalSprites.Register("lifedeath:redlily");

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

        // Create the scene's entities by cloning reference entities.
        public override void Load()
        {
            container = new Entity()
                .AttachTo(root);

            camera = new Entity()
                .SetRenderByDepth(true)
                .AttachTo(container);

            bCContainer1 = new Entity()
                .AttachTo(camera);

            bCContainer2 = new Entity()
                .AttachTo(camera);

            bCContainer3 = new Entity()
                .AttachTo(camera);

            new Entity(0, 0)
                .AddRenderComponent(new TextComponent(font, "Music Room"))
                .AttachTo(container);

            new Entity(0, 40)
                .AddRenderComponent(new TextComponent(font, "Use Up To Swith Between Songs And Down To Pause"))
                .AddUpdateComponent(new Components.MenuComponent())
                .AttachTo(container);
            new Entity(0, 80)
               .AddRenderComponent(new TextComponent(font, "Press Esc To Return To The Main Menu"))
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
