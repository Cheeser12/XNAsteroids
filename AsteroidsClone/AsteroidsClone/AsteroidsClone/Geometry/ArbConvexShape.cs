using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsClone
{
    /// <summary>
    /// A user-defined convex shape
    /// </summary>
    public class ArbConvexShape : Shape
    {
        /// <summary>
        /// Creates a user-defined convex shape
        /// </summary>
        /// <param name="pos">The position of the shape</param>
        /// <param name="verts">The vertices defining the shape, centered around the origin</param>
        /// <param name="indices">The indices defining the line segments that make up the shape</param>
        public ArbConvexShape(Vector2 pos, Vector2[] verts, short[] indices)
            : base(verts, indices)
        {
            Position = pos;
        }

        protected override void CreateShape()
        {
            // not needed, empty
        }

        public override object Clone()
        {
            Vector2[] verts = new Vector2[Vertices.Length];
            for (int i = 0; i < Vertices.Length; i++)
            {
                verts[i] = new Vector2(Vertices[i].Position.X, Vertices[i].Position.Y);
            }

            return new ArbConvexShape(Position, verts, Indices);
        }
    }
}
