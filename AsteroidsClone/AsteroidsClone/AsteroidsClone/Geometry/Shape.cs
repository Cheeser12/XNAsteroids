using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsClone
{
    /// <summary>
    /// Abstract class that defines a shape
    /// </summary>
    public abstract class Shape : ICloneable
    {
        private bool shapeCreated = false;
        public Vector2 Position { get; set; }

        private float rotation;
        public float Rotation
        {
            get
            {
                return rotation;
            }

            set
            {
                rotation = MathHelper.WrapAngle(value);
            }
        }

        public float Scale { get; set; }

        public VertexPositionColor[] Vertices { get; private set; }

        public Vector2[] TransformedVertices { get; private set; }
       

        public short[] Indices { get; private set; }


        public Matrix World { get; private set; }
        
        protected Shape(int sides)
        {
            Scale = 1f;
            Rotation = 0f;
            Vertices = new VertexPositionColor[sides];
            TransformedVertices = new Vector2[sides];

            Indices = new short[sides * 2];
        }

        protected Shape(Vector2[] verts, short[] indices)
        {
            if (indices.Length != (verts.Length * 2))
                throw new ArgumentException("Number of indices must be twice the number of vertices", "indices");
                    
            Scale = 1f;
            Rotation = 0f;

            Vertices = new VertexPositionColor[verts.Length];
            TransformedVertices = new Vector2[verts.Length];

            Indices = indices;

            for (int i = 0; i < verts.Length; i++)
            {
                Vertices[i] = new VertexPositionColor(new Vector3(verts[i], 0f), Color.White);
            }

            shapeCreated = true;
            
        }

        protected abstract void CreateShape();

        public virtual void Update(GameTime gameTime)
        {
            if (!shapeCreated)
            {
                CreateShape();
                shapeCreated = true;
            }

            World = Matrix.Identity;

            World *= Matrix.CreateScale(Scale);
            World *= Matrix.CreateRotationZ(Rotation);
            World *= Matrix.CreateTranslation(new Vector3(Position, 0f));

            // Get the transformed vertices
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vector3 position3d =
                    Vector3.Transform(Vertices[i].Position, World);

                TransformedVertices[i] = new Vector2(position3d.X, position3d.Y);
            }
        }

        public virtual bool Intersects(Shape s)
        {
            if (s is ConcaveShape)
                return (s as ConcaveShape).Intersects(this);

            if (s == this)
                return true;

            int i = 0;
            // Using this shape...
            for (i = 0; i < this.TransformedVertices.Length; i++)
            {
                // The next index should be i + 1
                // However, if we're on the last cycle, we have looped around.
                // The next index is the one we started with (0)
                int nextIndex = i + 1;
                if (i == this.TransformedVertices.Length - 1)
                    nextIndex = 0;

                Vector2 side = this.TransformedVertices[nextIndex] - this.TransformedVertices[i];

                // Find the axis perpendicular to the current side.
                // This is what we'll use to test the separation.
                Vector2 axis = new Vector2(side.Y, -side.X);

                Projection p1 = this.Project(axis);
                Projection p2 = s.Project(axis);

                if (!p1.Overlaps(p2))
                    return false;
            }

            // Next shape...
            for (i = 0; i < s.TransformedVertices.Length; i++)
            {
                // The next index should be i + 1
                // However, if we're on the last cycle, we have looped around.
                // The next index is the one we started with (0)
                int nextIndex = i + 1;
                if (i == s.TransformedVertices.Length - 1)
                    nextIndex = 0;

                Vector2 side = s.TransformedVertices[nextIndex] - s.TransformedVertices[i];

                // Find the axis perpendicular to the current side.
                // This is what we'll use to test the separation.
                Vector2 axis = new Vector2(side.Y, -side.X);

                Projection p1 = this.Project(axis);
                Projection p2 = s.Project(axis);

                if (!p1.Overlaps(p2))
                    return false;
            }

            return true;
        }

        public Projection Project(Vector2 axis)
        {
            // Make sure the axis is normalized
            axis.Normalize();

            float min = Vector2.Dot(axis,
                new Vector2(this.TransformedVertices[0].X, this.TransformedVertices[0].Y));

            float max = min;

            for (int i = 1; i < this.Vertices.Length; i++)
            {
                float p = Vector2.Dot(axis,
                    new Vector2(this.TransformedVertices[i].X, this.TransformedVertices[i].Y));

                if (p < min)
                    min = p;

                else if (p > max)
                    max = p;
            }

            return new Projection(min, max);
        }

        protected Vector2 GetPos(VertexPositionColor vert)
        {
            return new Vector2(vert.Position.X, vert.Position.Y);
        }

        public abstract object Clone();
     
    }
}
