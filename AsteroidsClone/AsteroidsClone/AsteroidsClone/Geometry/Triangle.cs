using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsClone
{
    public class Triangle : Shape
    {
        private Vector2[] points;
        
        /// <summary>
        /// NOTE: Three vectors should be considered centered around the origin
        /// AND should be in clockwise order so that the shape will draw
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public Triangle(Vector2 v1, Vector2 v2, Vector2 v3)
            : base(3)
        {
            points = new Vector2[3];
            points[0] = v1;
            points[1] = v2;
            points[2] = v3;
        }

        /// <summary>
        /// NOTE: Three vectors should be considered centered around the origin
        /// AND should be in clockwise order so that the shape will draw
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 pos)
            : base(3)
        {
            points = new Vector2[3];
            points[0] = v1;
            points[1] = v2;
            points[2] = v3;

            Position = pos;
        }

        protected override void CreateShape()
        {
            for (int i = 0; i < 3; i++)
            {
                Vertices[i] = new VertexPositionColor(
                    new Vector3(points[i], 0f), Color.White);
            }

            short[] indices =
            {
                0, 1,
                1, 2,
                2, 0
            };

            for (int i = 0; i < Indices.Length; i++)
            {
                Indices[i] = indices[i];
            }
        }

        private bool SameSide(Vector2 p1, Vector2 p2, Vector2 a, Vector2 b)
        {
            Vector3 cp1 = Vector3.Cross(new Vector3(b - a, 0f), new Vector3(p1 - a, 0f));
            Vector3 cp2 = Vector3.Cross(new Vector3(b - a, 0f), new Vector3(p2 - a, 0f));

            if (Vector3.Dot(cp1, cp2) >= 0f) return true;
            else return false;
        }

        public bool PointInTriangle(Vector2 p)
        {
            Vector2 a = points[0];
            Vector2 b = points[1];
            Vector2 c = points[2];

            if (SameSide(p, a, b, c) && SameSide(p, b, a, c) && SameSide(p, c, a, b)) return true;
            else return false;
        }

        public override object Clone()
        {
            return new Triangle(points[0], points[1], points[2]);              
        }
    }
}
