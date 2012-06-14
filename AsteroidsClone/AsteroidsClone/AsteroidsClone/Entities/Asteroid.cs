#region Using Statements

using Microsoft.Xna.Framework;
using System;

#endregion

namespace AsteroidsClone
{
    public enum AsteroidSize
    {
        Small,
        Normal,
        Large,
        Huge
    }

    /// <summary>
    /// Represents an asteroid
    /// </summary>
    public class Asteroid : Entity
    {
        #region Variables/Properties
        Random rand = new Random();
        public AsteroidSize Size { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an asteroid
        /// </summary>
        /// <param name="size">Size of the asteroid</param>
        /// <param name="verts">Vertices defining the shape of the asteroid</param>
        /// <param name="indices">Indices defining the shape of the asteroid</param>
        /// <param name="pos">Initial position of the asteroid</param>
        /// <param name="rot">Initial rotation of the asteroid (in radians)</param>
        /// <param name="dir">Initial direction of the asteroid</param>
        /// <param name="linSpeed">Linear speed of the asteroid</param>
        /// <param name="scale">Scale to apply to the vertices</param>
        public Asteroid(AsteroidSize size, Shape s,
            Vector2 pos, float rot, Vector2 dir, float linSpeed, float scale)
            : base(s, rot, dir, scale)
        {
            Position = pos;
            Size = size;
            LinSpeed = linSpeed;
        }

        public Asteroid[] Destroy()
        {
            if (Size == AsteroidSize.Small)
                return new Asteroid[0];


            else if (Size == AsteroidSize.Normal)
            {
                // Generate two small asteroids
                float angle = (float)(10 * rand.Next(-9, 9));
                Vector3 firstVector = Vector3.Transform(
                    new Vector3(Direction, 0f),
                    Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));

                firstVector.Normalize();
                Vector3 secondVector = -firstVector;

                Asteroid a1 = new Asteroid(AsteroidSize.Small, (Shape)Body.Clone(),
                    Position, Rotation, new Vector2(firstVector.X, firstVector.Y), LinSpeed, Scale * 0.5f);

                Asteroid a2 = new Asteroid(AsteroidSize.Small, (Shape)Body.Clone(), Position, Rotation,
                    new Vector2(secondVector.X, secondVector.Y), LinSpeed, Scale * 0.5f);

                return new Asteroid[] { a1, a2 };

            }

            else if (Size == AsteroidSize.Large)
            {
                float angle = (float)(10 * rand.Next(-9, 9));
                Vector3 firstVector = Vector3.Transform(
                    new Vector3(Direction, 0f),
                    Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
                firstVector.Normalize();

                Vector3 secondVector = Vector3.Transform(
                    new Vector3(Direction, 0f),
                    Matrix.CreateRotationZ(MathHelper.ToRadians(-angle)));
                secondVector.Normalize();

                Vector3 thirdVector = -firstVector;
                Vector3 fourthVector = -secondVector;

                Asteroid a1 = new Asteroid(AsteroidSize.Small, (Shape)Body.Clone(), Position,
                    Rotation, new Vector2(firstVector.X, firstVector.Y), LinSpeed, Scale * 0.42f);

                Asteroid a2 = new Asteroid(AsteroidSize.Small, (Shape)Body.Clone(), Position,
                    Rotation, new Vector2(secondVector.X, secondVector.Y), LinSpeed, Scale * 0.42f);

                Asteroid a3 = new Asteroid(AsteroidSize.Small, (Shape)Body.Clone(), Position,
                    Rotation, new Vector2(thirdVector.X, thirdVector.Y), LinSpeed, Scale * 0.42f);

                Asteroid a4 = new Asteroid(AsteroidSize.Small, (Shape)Body.Clone(), Position,
                    Rotation, new Vector2(fourthVector.X, fourthVector.Y), LinSpeed, Scale * 0.42f);

                return new Asteroid[] { a1, a2, a3, a4 };

            }

            else
            {
                float angle = (float)(10 * rand.Next(-9, 9));
                Vector3 firstVector = Vector3.Transform(
                    new Vector3(Direction, 0f),
                    Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
                firstVector.Normalize();

                Vector3 secondVector = Vector3.Transform(
                    new Vector3(Direction, 0f),
                    Matrix.CreateRotationZ(MathHelper.ToRadians(-angle)));
                secondVector.Normalize();

                Vector3 thirdVector = -firstVector;
                Vector3 fourthVector = -secondVector;

                Asteroid a1 = new Asteroid(AsteroidSize.Normal, (Shape)Body.Clone(),
                    Position, Rotation, new Vector2(firstVector.X, firstVector.Y), LinSpeed, Scale * 0.71f);

                Asteroid a2 = new Asteroid(AsteroidSize.Normal, (Shape)Body.Clone(), Position, Rotation,
                    new Vector2(secondVector.X, secondVector.Y), LinSpeed, Scale * 0.71f);

                Asteroid a3 = new Asteroid(AsteroidSize.Normal, (Shape)Body.Clone(), Position, Rotation,
                   new Vector2(thirdVector.X, thirdVector.Y), LinSpeed, Scale * 0.71f);

                Asteroid a4 = new Asteroid(AsteroidSize.Normal, (Shape)Body.Clone(), Position, Rotation,
                   new Vector2(fourthVector.X, fourthVector.Y), LinSpeed, Scale * 0.71f);

                return new Asteroid[] { a1, a2, a3, a4 };
            }
           
        }

        #endregion


    }
}
