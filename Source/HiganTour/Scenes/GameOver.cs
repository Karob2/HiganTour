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
    public class GameOver : Scene
    {
        Font font;

        // Create a reference set of entities and load necessary assets.
        public override void Preload(Entity root)
        {
            this.root = root;

            font = GlobalServices.GlobalFonts.Register("higantour:sans");
        }

        // Create the scene's entities by cloning reference entities.
        public override void Load()
        {
            sceneContainer = new Entity();
            sceneContainer.AttachTo(root);

            new Entity(0, 0)
                .AddRenderComponent(new TextComponent(font, "Game Over"))
                .AttachTo(sceneContainer);

            new Entity(0, 40)
                .AddRenderComponent(new TextComponent(font, "Press Enter to Try Again"))
                .AddUpdateComponent(new Components.MenuComponent())
                .AttachTo(sceneContainer);
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
