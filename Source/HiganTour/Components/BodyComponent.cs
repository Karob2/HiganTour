using System;
using System.Collections.Generic;
using System.Text;
using Lichen.Entities;
using Microsoft.Xna.Framework;

namespace HiganTour.Components
{
    /*
    public enum CollisionTypes
    {
        Box,
        Circle,
        Polygon
    }
    */

    public class BodyComponent : Component
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public Rectangle RenderArea { get; set; }
        /*
        public CollisionTypes CollisionType { get; set; }
        public Rectangle CollisionRect { get; set; }
        public Vector3 CollisionCircle { get; set; }
        public Vector4 CollisionSphere { get; set; }
        public Polygon CollisionPolygon { get; set; }
        public Polyhedron CollisionPolyhedron { get; set; }
        */
        //public CollisionBody CollisionBody { get; set; }

        public override void OnAttach()
        {
        }

        public override void AttachTo(Entity entity)
        {
            entity.AddComponent(this);
        }
    }

    /*
    public class CollisionBody
    {
        CollisionTypes type { get; set; }

        Rectangle rect { get; set; }
        Vector3 circle { get; set; }
        Polygon polygon { get; set; }

        Vector6 box { get; set; }
        Vector4 sphere { get; set; }
        Polyhedron polyhedron { get; set; }

        // If circle, generate polygon alternative.
        // Have function to auto generate ellipse polygon.
        // For now, could just simplify all collision bodies to be polygons.
    }
    */
}
