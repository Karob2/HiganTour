using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lichen.Entities;

namespace LifeDeath.Components
{
    class CameraComponent : Lichen.Entities.Component, Lichen.Entities.IUpdateComponent
    {
        Entity target;

        public CameraComponent(Entity target)
        {
            this.target = target;
        }

        public void Update()
        {
            //Owner.X = -target.X + Lichen.GlobalServices.Game.GraphicsDevice.Viewport.Width / 2;
            Owner.X = 0;
            Owner.Y = -target.Y + Lichen.GlobalServices.Game.GraphicsDevice.Viewport.Height / 2;
        }
    }
}
