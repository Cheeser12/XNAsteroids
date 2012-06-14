using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsClone
{
    /// <summary>
    /// Rectangle that uses floating point values.
    /// </summary>
    public sealed class Rect : Shape
    {
        // CENTER BASED
        

        public float Height { get; private set; }
        public float Width { get; private set; }

        public static Rect Empty
        {
            get
            {
                return new Rect(Vector2.Zero, 0f, 0f);
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (Height == 0f && Width == 0f);
            }
        }


        public Rect(Vector2 pos, float height, float width)
            : base(4)
        {
            Position = pos;
            Height = height;
            Width = width;
        }

        protected override void CreateShape()
        {
            Vector2[] vertPos = new Vector2[4];

            // CLOCKWISE --
            // TOP RIGHT
            vertPos[0] = new Vector2(Width / 2f, Height / 2f);
            // BOTTOM RIGHT
            vertPos[1] = new Vector2(Width / 2f, -Height / 2f);
            // BOTTOM LEFT
            vertPos[2] = new Vector2(-Width / 2f, -Height / 2f);
            // TOP LEFT
            vertPos[3] = new Vector2(-Width / 2f, Height / 2f);

            for (int i = 0; i < 4; i++)
            {
                Vertices[i] = new VertexPositionColor(new Vector3(vertPos[i], 0f), Color.White);
            }

            short[] indices =
            {
                0, 1,
                1, 2,
                2, 3,
                3, 0
            };

            for (int i = 0; i < indices.Length; i++)
            {
                Indices[i] = indices[i];
            }
        }

        public override string ToString()
        {
            return "Pos: " + Position.ToString() + " He. = " + Height + " Wi. = " + Width;
        }

        public override object Clone()
        {
            return new Rect(Position, Height, Width);
        }
    }
}
