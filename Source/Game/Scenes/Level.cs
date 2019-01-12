using Lichen;
using Lichen.Entities;
using Lichen.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeDeath.Scenes
{
    public class Level : Scene
    {
        Entity camera;
        Entity player;
        Entity enemy;
        Entity lycoris;
        //Entity zone1, zone2, zone3;

        List<Entity> actorList;

        Font font;

        // Create a reference set of entities and load necessary assets.
        public override void Preload(Entity root)
        {
            this.root = root;
            actorList = new List<Entity>();

            player = new Entity()
                .AddRenderComponent(new SpriteComponent(GlobalServices.GlobalSprites.Register("lifedeath:spirit")))
                .AddChainComponent("control", new Components.PlayerControlComponent());
            //.AddChainComponent("motion", )
            /*
            new Entity(0, 0)
                .AddRenderComponent(new TextComponent(GlobalServices.GlobalFonts.Register("lifedeath:sans"), "Player 1"))
                .AttachTo(player);
                */
            enemy = new Entity()
                .AddRenderComponent(new SpriteComponent(GlobalServices.GlobalSprites.Register("lifedeath:darkness")))
                .AddChainComponent("control", new Components.AI.SeekerAIComponent(player));

            Sprite lycorisSprite = GlobalServices.GlobalSprites.Register("lifedeath:lycoris");
            lycoris = new Entity()
                .AddRenderComponent(new SpriteComponent(lycorisSprite));

            font = GlobalServices.GlobalFonts.Register("lifedeath:sans");
        }

        // Create the scene's entities by cloning reference entities.
        public override void Load()
        {
            container = new Entity()
                .AttachTo(root);

            camera = new Entity()
                .SetRenderByDepth(true)
                .AddUpdateComponent(new Components.CameraComponent(player))
                .AttachTo(container);

            /*
            zone1 = new Entity().AttachTo(container);
            zone2 = new Entity().AttachTo(container);
            zone3 = new Entity().AttachTo(container);
            */

            Random random = new Random();
            double phi = (Math.Sqrt(5d)-1d)/2d;
            double theta = random.NextDouble();
            for (int i = 0; i < 200; i++)
            {
                lycoris.Clone()
                    //.SetPosition(random.Next(0, 700), random.Next(0, 700))
                    //.AddChainComponent("motion", new Components.WindyComponent(random.Next(0, 1280), random.Next(0, 720)))
                    .AddChainComponent("motion", new Components.WindyComponent(camera, (float)(theta * 1280d + random.NextDouble() * 200d - 100d), (float)i * 720f / 200f))
                    .AttachTo(camera)
                    .AddActorList(actorList);
                theta += phi;
                if (theta > 1d) theta -= 1d;
            }

            //enemy.Clone().SetPosition(200, 200).AttachTo(camera)
            enemy.SetPosition(200, 200).AttachTo(camera)
                .AddActor(actorList);

            //Entity player1 = player.Clone().SetPosition(300, 200).AttachTo(container);
            player.SetPosition(300, 200).AttachTo(camera)
                .AddActor(actorList);

            /*
            TextComponent tc = (TextComponent)player1.Children.First.Value.RenderComponent;
            tc.Value = "PLAYER";
            */
        }

        // Delete the scene. (Reference entities and assets remain.)
        public override void Unload()
        {
            // TODO: Should this also undo the preloading?
            container = null;
            // TODO: Is that enough to destroy the scene entities? Or do I need to parse through them all?
            //     Is garbage collection hindered by parent and child referencing each other?
        }

        public void Reset()
        {
            enemy.SetPosition(200, 200);
            player.SetPosition(300, 200);
        }
    }
}
