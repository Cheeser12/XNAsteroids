using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsClone
{
    public class Renderer
    {
        private Matrix view;
        private Matrix proj;

        private BasicEffect effect;
        private GraphicsDevice gDevice;

        public Renderer(GraphicsDevice gDevice)
        {
            this.gDevice = gDevice;
          
            view = Matrix.Identity;
            //view = Matrix.CreateLookAt(new Vector3(0f, 0f, 50f), Vector3.Zero, Vector3.Up);
            //proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), gDevice.DisplayMode.AspectRatio,
            //    1f, 1000f);

            proj = Matrix.CreateOrthographicOffCenter(0, gDevice.Viewport.Width,
                gDevice.Viewport.Height, 0f, -1f, 1f);

            effect = new BasicEffect(gDevice);
            effect.VertexColorEnabled = true;
            effect.View = view;
            effect.Projection = proj;

            
        }

        public void Draw(Shape s)
        {
            effect.World = s.World;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                gDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList,
                    s.Vertices, 0, s.Vertices.Length, s.Indices, 0, s.Indices.Length / 2);
            }
        }
    }
}
