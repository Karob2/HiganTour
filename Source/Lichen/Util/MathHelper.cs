using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;

namespace Lichen.Util
{
    public class MathHelper
    {
        public static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        public static double Distance(Entity entity1, Entity entity2)
        {
            return Math.Sqrt(Math.Pow(entity1.X - entity2.X, 2) + Math.Pow(entity1.Y - entity2.Y, 2));
        }
    }
}
