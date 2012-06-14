using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AsteroidsClone
{
    /// <summary>
    /// Class that contains asteroid shapes to be chosen at random
    /// </summary>
    public static class AsteroidGenerator
    {
        static ArbConvexShape[] convexAsteroids;
        static AsteroidGenerator()
        {
            // Generate convex asteroids
            convexAsteroids = new ArbConvexShape[5];

            convexAsteroids[0] = new ArbConvexShape(Vector2.Zero,
                new Vector2[]{
                        new Vector2(0.1f, -0.2f),
                        new Vector2(0.3f, -0.15f),
                        new Vector2(0.32f, 0.1f),
                        new Vector2(0.08f, 0.2f),
                        new Vector2(-0.21f, 0.13f),
                        new Vector2(-0.3f, -0.11f),
                        new Vector2(-0.1f, -0.15f)
                    }, GenerateIndices(7));

            convexAsteroids[1] = new ArbConvexShape(Vector2.Zero,
                new Vector2[]{
                        new Vector2(-0.3f, 0.5f),
                        new Vector2(0.1f, 0.4f),
                        new Vector2(0.4f, 0.1f),
                        new Vector2(0.5f, -0.2f),
                        new Vector2(0f, -0.5f),
                        new Vector2(-0.3f, -0.3f),
                        new Vector2(-0.5f, 0.2f)
                    }, GenerateIndices(7));

            convexAsteroids[2] = new ArbConvexShape(Vector2.Zero,
                new Vector2[]{
                        new Vector2(-0.1f, 0.4f),
                        new Vector2(0.35f, 0.3f),
                        new Vector2(0.5f, 0f),
                        new Vector2(0.3f, -0.3f),
                        new Vector2(-0.25f, -0.3f),
                        new Vector2(-0.3f, 0.2f)
                    }, GenerateIndices(6));

            convexAsteroids[3] = new ArbConvexShape(Vector2.Zero,
                new Vector2[]{
                        new Vector2(0.1f, 0.4f),
                        new Vector2(0.25f, 0.3f),
                        new Vector2(0.4f, 0f),
                        new Vector2(0.25f, -0.3f),
                        new Vector2(-0.05f, -0.5f),
                        new Vector2(-0.45f, -0.2f),
                        new Vector2(-0.35f, 0.1f)
                    }, GenerateIndices(7));

            convexAsteroids[4] = new ArbConvexShape(Vector2.Zero,
                new Vector2[]{
                        new Vector2(-0.05f, 0.4f),
                        new Vector2(0.2f, 0.4f),
                        new Vector2(0.3f, 0.3f),
                        new Vector2(0.45f, 0.2f),
                        new Vector2(0.4f, -0.1f),
                        new Vector2(0.25f, -0.3f),
                        new Vector2(0.05f, -0.4f),
                        new Vector2(-0.3f, -0.3f),
                        new Vector2(-0.5f, 0f),
                        new Vector2(-0.25f, 0.3f)
                }, GenerateIndices(10));
                

                

        }

        /// <summary>
        /// Generates an asteroid with a convex body
        /// </summary>
        /// <param name="rand">Random number generator</param>
        /// <param name="pos">Initial position of the asteroid</param>
        /// <param name="rot">Initial rotation of the asteroid (in radians)</param>
        /// <param name="linSpeed">Speed of the asteroid</param>
        /// <param name="dir">Initial direction of the asteroid</param>
        /// <returns>An asteroid with a convex body</returns>
        public static Asteroid GetConvexAsteroid(int index, Vector2 pos, float rot, float linSpeed, Vector2 dir, AsteroidSize size)
        {

            Shape s;

          
            float scale;
            float effectiveWidth;
            float effectiveHeight;

            // Use the index to find a random asteroid shape
            // The scale, effectiveWidth, and effectiveHeight are based on an asteroid of normal size
            switch (index)
            {
                case 0:
                    s = convexAsteroids[0];                   
                    effectiveHeight = 40f;
                    effectiveWidth = 45f;
                    scale = 150f;
                    break;

                case 1:
                    s = convexAsteroids[1];
                    effectiveHeight = 60f;
                    effectiveWidth = 60f;
                    scale = 80f;
                    break;

                case 2:
                    s = convexAsteroids[2];
                    effectiveHeight = 40f;
                    effectiveWidth = 40f;
                    scale = 80f;
                    break;

                case 3:
                    s = convexAsteroids[3];
                    effectiveHeight = 40f;
                    effectiveWidth = 40f;
                    scale = 75f;
                    break;

                case 4:
                    s = convexAsteroids[4];
                    effectiveHeight = 40f;
                    effectiveWidth = 40f;
                    scale = 75f;
                    break;


                default:
                    throw new ArgumentOutOfRangeException();
                  
            }

            // If the asteroid size isn't normal, we adjust the size variables
            switch (size)
            {
                case AsteroidSize.Small:
                    scale *= 0.5f;
                    effectiveHeight *= 0.6f;
                    effectiveWidth *= 0.6f;
                    break;

                case AsteroidSize.Large:
                    scale *= 1.2f;
                    effectiveHeight *= 1.1f;
                    effectiveWidth *= 1.1f;
                    break;

                case AsteroidSize.Huge:
                    scale *= 1.5f;
                    effectiveHeight *= 1.4f;
                    effectiveWidth *= 1.4f;
                    break;

                default:
                    break;

            }


            // Return the asteroid
            Asteroid a = new Asteroid(size, s, pos, rot, dir, linSpeed, scale);
            a.EffectiveHeight = effectiveHeight;
            a.EffectiveWidth = effectiveWidth;

            return a;
            
        }

        /// <summary>
        /// Helper class for generating an index array
        /// </summary>
        /// <param name="vertsLength">Length of vertex array</param>
        /// <returns>An index array corresponding to the vertex array</returns>
        static short[] GenerateIndices(int vertsLength)
        {
            // Number of indices should be 2 * vertsLength
            short[] indices = new short[vertsLength * 2];

            // Loop to generate the array
            for (int i = 0; i < indices.Length; i += 2)
            {
                if (i == 0)
                {
                    indices[i] = 0;
                    indices[i + 1] = 1;
                }

                else if (i == indices.Length - 2)
                {
                    indices[i] = indices[i - 1];
                    indices[i+1] = 0;
                }

                else
                {
                    indices[i] = indices[i - 1];
                    indices[i + 1] = (short)(indices[i - 1] + 1);
                }

                
                
            }

            return indices;
        }

        
    }
}
