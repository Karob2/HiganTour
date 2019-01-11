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
        Entity block;
        Entity player;

        Font font;

        // Create a reference set of entities and load necessary assets.
        public override void Preload(Entity root)
        {
            this.root = root;

            // TODO: Add SpriteLibrary.Lookup(string) when the asset should not be loaded if not found.
            Sprite blockSprite = GlobalServices.GlobalSprites.Register("lifedeath:block_gray");
            block = new Entity()
                .AddRenderComponent(new SpriteComponent(blockSprite));
            player = new Entity()
                .AddRenderComponent(new SpriteComponent(GlobalServices.GlobalSprites.Register("lifedeath:hopper")))
                .AddChainComponent("control", new Components.PlayerControlComponent());
            //.AddChainComponent("motion", )
            new Entity(0, 0)
                .AddRenderComponent(new TextComponent(GlobalServices.GlobalFonts.Register("lifedeath:sans"), "Player 1"))
                .AttachTo(player);

            font = GlobalServices.GlobalFonts.Register("lifedeath:sans");
        }

        // Create the scene's entities by cloning reference entities.
        public override void Load()
        {
            container = new Entity();
            container.AttachTo(root);

            block.Clone().SetPosition(200, 200).AttachTo(container);
            Entity player1 = player.Clone().SetPosition(300, 200).AttachTo(container);

            TextComponent tc = (TextComponent)player1.Children.First.Value.RenderComponent;
            tc.Value = "PLAYER";
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
