using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AsteroidsClone
{
    /// <summary>
    /// Defines the sides of the screen
    /// </summary>
    public enum ScreenSide
    {
        Top,
        Bottom,
        Left,
        Right
    };

    public class EntityManager
    {
        private enum XBounds
        {
            WithinScreen,
            RightOfScreen,
            LeftOfScreen
        };

        private enum YBounds
        {
            WithinScreen,
            AboveScreen,
            BelowScreen
        };

        private Random rand;
        private readonly float screenWidth;
        private readonly float screenHeight;
        public List<Asteroid> ActiveAsteroids { get; private set; }
        public Player Player { get; private set; }

        public EntityManager(float screenWidth, float screenHeight)
        {
            rand = new Random();
            ActiveAsteroids = new List<Asteroid>();
            Player = new Player(new Vector2(300f, 300f), 0f);
            
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        public void Update(GameTime gameTime)
        {
            // Update the asteroids...
            foreach (Asteroid a in ActiveAsteroids)
            {
                a.Update(gameTime);

                if (!a.Reflected)
                {
                    a.ReflectAcrossScreen(screenWidth, screenHeight);
                }
            }

            // ...the player...
            Player.Update(gameTime);
            CheckBulletHits();
            if (!Player.Reflected)
            {
                Player.ReflectAcrossScreen(screenWidth, screenHeight);
            }

            // Add new asteroids?
            if (ActiveAsteroids.Count < 1)
                GenerateNewAsteroid();
        }

        private void CheckBulletHits()
        {
            for (int i = 0; i < Player.ActiveBullets.Count; i++)
            {
                for (int j = 0; j < ActiveAsteroids.Count; j++)
                {
                    if (i >= Player.ActiveBullets.Count)
                        break;

                    if (Player.ActiveBullets[i].HitEntity(ActiveAsteroids[j]))
                    {
                        Player.ActiveBullets.RemoveAt(i);                     
                        ActiveAsteroids.AddRange(ActiveAsteroids[j].Destroy());
                        ActiveAsteroids.RemoveAt(j);                  
                    }
                }
            }
        }

        public void Draw(Renderer rend)
        {
            // Draw the asteroids...
            foreach (Asteroid a in ActiveAsteroids)
            {
                a.Draw(rend);
            }

            //...the player...
            Player.Draw(rend);
        }

        void GenerateNewAsteroid()
        {
            // Find where to generate the asteroid
            int xBoundsNumber = rand.Next(0, 2);
            XBounds xBound;

            int yBoundsNumber = rand.Next(0, 2);
            YBounds yBound;

            switch (xBoundsNumber)
            {
                case 0:
                    xBound = XBounds.WithinScreen;
                    break;

                case 1:
                    xBound = XBounds.RightOfScreen;
                    break;

                case 2:
                    xBound = XBounds.LeftOfScreen;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (xBound == XBounds.WithinScreen)
            {
                yBoundsNumber = rand.Next(0, 1);
                switch (yBoundsNumber)
                {
                    case 0:
                        yBound = YBounds.AboveScreen;
                        break;

                    case 1:
                        yBound = YBounds.BelowScreen;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();

                }
            }

            else
            {
                switch (yBoundsNumber)
                {
                    case 0:
                        yBound = YBounds.WithinScreen;
                        break;

                    case 1:
                        yBound = YBounds.AboveScreen;
                        break;

                    case 2:
                        yBound = YBounds.BelowScreen;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Generate coords
            float x;
            float y;

            switch(xBound)
            {
                case XBounds.WithinScreen:
                    x = (float)rand.Next(0, (int)screenWidth);
                    break;

                case XBounds.LeftOfScreen:
                    x = (float)rand.Next(-11, 0);
                    break;

                case XBounds.RightOfScreen:
                    x = (float)rand.Next((int)screenWidth, (int)screenWidth + 11);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (yBound)
            {
                case YBounds.WithinScreen:
                    y = (float)rand.Next(0, (int)screenHeight);
                    break;

                case YBounds.AboveScreen:
                    y = (float)rand.Next(-11, 0);
                    break;

                case YBounds.BelowScreen:
                    y = (float)rand.Next((int)screenHeight, (int)screenHeight + 11);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Vector2 pos = new Vector2(x, y);
            
            // Generate direction by picking a random point on the screen
            float screenX = (float)rand.Next(0, (int)screenWidth);
            float screenY = (float)rand.Next(0, (int)screenHeight);
            Vector2 dir = new Vector2(screenX, screenY);
            dir.Normalize();
            
            // Generate a speed
            float speed = (float)rand.Next(1, 3);

            // Create the asteroid
            //Asteroid a = AsteroidGenerator.GetConvexAsteroid(rand, pos, 0f, speed, dir);
            //ActiveAsteroids.Add(a);

#warning test
            Asteroid a = AsteroidGenerator.GetConvexAsteroid(2, new Vector2(400f, 300f), 0f, 0.5f, Vector2.UnitX, AsteroidSize.Huge);
            ActiveAsteroids.Add(a);
            
        }       

        
    }
}
