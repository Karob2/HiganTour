using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lichen.Entities;

namespace HiganTour.Components
{
    class CameraComponent : Lichen.Entities.Component //, Lichen.Entities.IUpdateComponent
    {
        public string TargetName { get; set; }

        public CameraComponent(string targetName)
        {
            TargetName = targetName;
        }

        /*
        public void Update()
        {
            Entity target = Owner.Scene.GetEntity(targetName);
            if (target == null) return;
            //Owner.X = -target.X + Lichen.GlobalServices.Game.GraphicsDevice.Viewport.Width / 2;
            Owner.X = 0;
            Owner.Y = -target.Y + Lichen.GlobalServices.Game.GraphicsDevice.Viewport.Height / 2f + 100f;
        }
        */

        public override void AttachTo(Entity entity)
        {
            entity.AddComponent(this);
        }
    }
}
