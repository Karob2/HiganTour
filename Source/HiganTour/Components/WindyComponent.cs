using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lichen.Entities;

namespace HiganTour.Components
{
    public class WindyComponent : Component //, Lichen.Entities.IUpdateComponent
    {
        /*
        double x, y;
        double dx, dy;
        double vx, vy;
        double xx, yy;
        */
        //public double X { get; set; }
        //public double Y { get; set; }
        public double OldX { get; set; }
        public double OldY { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double Dx { get; set; }
        public double Dy { get; set; }
        public double Vx { get; set; }
        public double Vy { get; set; }

        //bool bound;

        public WindyComponent()
        {
        }

        /*
        public WindyComponent(float x, float y)
        {
            X = x;
            Y = y;
            bound = true;
        }
        */

        public override void OnAttach()
        {
            /*
            if (!bound)
            {
                X = Owner.X;
                Y = Owner.Y;
                bound = true;
            }
            base.OnAttach();
            */
        }

        public override void AttachTo(Entity entity)
        {
            entity.AddComponent(this);
        }
    }
}
