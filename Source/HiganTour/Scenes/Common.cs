using Lichen;
using Lichen.Entities;
using Lichen.Entities.Components;
using Lichen.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiganTour.Scenes
{
    public static class Common
    {
        static Entity lycoris;
        public static Entity Lycoris { get { return lycoris; } }

        public static void Preload()
        {
            lycoris = new Entity()
                .SetRenderLayer(-1)
                .AddComponent(new Components.BodyComponent())
                .AddComponent(new SpriteComponent(GlobalServices.GlobalSprites.Register("higantour:redlily")))
                .AddComponent(new Components.WindyComponent());

            //GlobalServices.GlobalSprites.Register("higantour:redlily");
            /*
            Sprite lycorisSprite = GlobalServices.GlobalSprites.Register("higantour:redlily");
            Entity lycoris = new Entity()
                .AddRenderComponent(new SpriteComponent(lycorisSprite));

            Entity lycorisField = new Entity();
            Random random = new Random();
            double phi = (Math.Sqrt(5d) - 1d) / 2d;
            double theta = random.NextDouble();
            for (int i = 0; i < 200; i++)
            {
                lycoris.Clone()
                    //.SetPosition(random.Next(0, 700), random.Next(0, 700))
                    //.AddChainComponent("motion", new Components.WindyComponent(random.Next(0, 1280), random.Next(0, 720)))
                    .AddChainComponent("motion", new Components.WindyComponent((float)(theta * 1280d + random.NextDouble() * 200d - 100d), (float)i * 920f / 200f))
                    .AttachTo(lycorisField);
                theta += phi;
                if (theta > 1d) theta -= 1d;
            }
            GlobalServices.EntityLibrary.Add("lycoris-field", lycorisField);
            */
        }
    }
}
