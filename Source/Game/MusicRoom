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

        

        // Create a reference set of entities and load necessary assets.
        public override void Preload(Entity root)
        {
            this.root = root;

            font = GlobalServices.GlobalFonts.Register("lifedeath:sans");
        }

        // Create the scene's entities by cloning reference entities.
        public override void Load()
        {

            container = new Entity();
            container.AttachTo(root);

            new Entity(0, 0)
                .AddRenderComponent(new TextComponent(font, "Music Room"))
                .AttachTo(container);

            new Entity(0, 40)
                .AddRenderComponent(new TextComponent(font, "Use W To Swith Between Songs And Use S To Pause"))
                .AddUpdateComponent(new Components.MenuComponent())
                .AttachTo(container);
            new Entity(0, 80)
               .AddRenderComponent(new TextComponent(font, "Press Esc To Return To The Main Menu"))
               .AddUpdateComponent(new Components.MenuComponent())
               .AttachTo(container);
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
