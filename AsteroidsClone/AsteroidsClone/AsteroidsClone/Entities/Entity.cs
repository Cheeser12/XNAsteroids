#region Using Statements

using System;
using Microsoft.Xna.Framework;

#endregion

namespace AsteroidsClone
{
    /// <summary>
    /// Abstract class defining an entity within the game
    /// </summary>
    public abstract class Entity
    {
        #region Variables/Properties

        // Minimum time between reflects across the screen
        private const float reflectGracePeriod = 1.5f;

        // Time since the last reflect
        private float timeSinceReflect = 0f;

        // Effective width and height represent the average height and width of the entity
        // They are used to determine at what position the entity is considered "out of bounds"
        /// <summary>
        /// Effective width of the entity
        /// </summary>
        public float EffectiveWidth { get; set; }

        /// <summary>
        /// Effective height of the entity
        /// </summary>
        public float EffectiveHeight { get; set; }

        /// <summary>
        /// The shape that defines the body of the entity
        /// </summary>
        public Shape Body { get; private set; }

        /// <summary>
        /// The position of the entity
        /// </summary>
        public Vector2 Position { get; protected set; }

        private float rot;
        /// <summary>
        /// The rotation of the entity (in radians)
        /// </summary>
        public float Rotation
        {
            get { return rot; }
            protected set
            {
                // Wrap the angle to ensure rot never goes out of bounds
                rot = MathHelper.WrapAngle(value);
            }
        }

        /// <summary>
        /// The linear speed of the entity
        /// </summary>
        public float LinSpeed { get; protected set; }

        // The direction specifies where the entity is "facing" and is mainly used in movement.
        private Vector2 dir;
        /// <summary>
        /// The current direction of the entity.
        /// </summary>
        public Vector2 Direction
        {
            get { return dir; }
            protected set
            {
                // Ensure the direction is always a unit length vector
                Vector2 newDir = value;
                newDir.Normalize();
                dir = newDir;
            }
        }

        /// <summary>
        /// The scale of the entity
        /// </summary>
        public float Scale { get; protected set; }

        /// <summary>
        /// Shows whether the entity has been recently reflected
        /// </summary>
        public bool Reflected { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs the base of an entity
        /// </summary>
        /// <param name="body">Shape defining the body of the entity</param>
        /// <param name="rot">The initial rotation of the entity</param>
        protected Entity(Shape body, float rot)
        {
            Body = body;
            Position = Body.Position;
            Rotation = rot;
            LinSpeed = 0f;
            Scale = 1f;

            // Find the initial direction, based on the rotation
            Direction = new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot));

            // Initial values
            EffectiveHeight = 15f;
            EffectiveWidth = 15f;
        }
        
        /// <summary>
        /// Constructs the base of an entity
        /// </summary>
        /// <param name="body">Shape defining the body of the entity</param>
        /// <param name="rot">The initial rotation of the entity</param>
        /// <param name="dir">Initial direction of the entity</param>
        /// <param name="scale">Scale of the entity</param>
        public Entity(Shape body, float rot, Vector2 dir, float scale)
        {
            Body = body;
            Position = Body.Position;
            Rotation = rot;
            LinSpeed = 0f;
            Scale = scale;
            Direction = dir;

            EffectiveHeight = 15f;
            EffectiveWidth = 15f;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <param name="gameTime">Timespan of the game</param>
        public virtual void Update(GameTime gameTime)
        {
            // Move the entity, based on the direction of movement and the speed.
            Position += (Direction * LinSpeed);

            // Update all of the body variables
            Body.Position = Position;
            Body.Rotation = Rotation;
            Body.Scale = Scale;
            Body.Update(gameTime);
            
            // If the entity has recently been reflected...
            if (Reflected)
            {
                // Add the time since the last update to the time since last reflect
                timeSinceReflect += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // If the time since reflect is greater than the grace period, the shape can be reflected again
                if (timeSinceReflect > reflectGracePeriod)
                {
                    Reflected = false;
                    timeSinceReflect = 0f;
                }
            }
        }

        /// <summary>
        /// Draws the entity.
        /// </summary>
        /// <param name="rend">Renderer to use for drawing</param>
        public virtual void Draw(Renderer rend)
        {
            rend.Draw(Body);
        }

        /// <summary>
        /// Reflects the entity across the screen.
        /// </summary>
        /// <param name="screenWidth">The width of the screen.</param>
        /// <param name="screenHeight">The height of the screen.</param>
        public virtual void ReflectAcrossScreen(float screenWidth, float screenHeight)
        {
            // We start by checking the x bounds of the entity
            
            // Entity is past.. 
            
            //...the RIGHT side of the screen.
            if (Position.X - EffectiveWidth > screenWidth)
            {
                // newY is a nullable float. We need to keep it null if the entity has
                // exceeded the x bounds, but NOT the y bounds.
                float? newY = null;

                // Find the new x-coordinate after reflecting across the screen horizontally
                float newX = GetReflectCoord(ScreenSide.Right, screenWidth, screenHeight);

                // Now check if the y-bounds have been exceeded as well.
                // If not, newY remains null.
                if (Position.Y + EffectiveHeight < 0f)
                {
                    newY = GetReflectCoord(ScreenSide.Top, screenWidth, screenHeight);
                }

                else if (Position.Y - EffectiveHeight > screenHeight)
                {
                    newY = GetReflectCoord(ScreenSide.Bottom, screenWidth, screenHeight);
                }

                // Now we need to find the new position.
                // If newY is still null, we use the current y position....if it isn't ,we use newY instead
                // We also mark that the entity has just been reflected
                Vector2 newPos = (newY == null) ? new Vector2(newX, Position.Y) : new Vector2(newX, (float)newY);
                Position = newPos;
                Reflected = true;
            }

            // ...the LEFT side of the screen
            else if (Position.X + EffectiveWidth < 0f)
            {
                float? newY = null;
                float newX = GetReflectCoord(ScreenSide.Left, screenWidth, screenHeight);

                if (Position.Y + EffectiveHeight < 0f)
                {
                    newY = GetReflectCoord(ScreenSide.Top, screenWidth, screenHeight);
                }

                else if (Position.Y - EffectiveHeight > screenHeight)
                {
                    newY = GetReflectCoord(ScreenSide.Bottom, screenWidth, screenHeight);
                }

                Vector2 newPos = (newY == null) ? new Vector2(newX, Position.Y) : new Vector2(newX, (float)newY);
                Position = newPos;
                Reflected = true;
            }

            //...the TOP of the screen
            else if (Position.Y + EffectiveHeight < 0f)
            {
                // We only need to reflect vertically here, since we checked x-bounds above...
                // Just use the current x-coordinate in the new position
                float newY = GetReflectCoord(ScreenSide.Top, screenWidth, screenHeight);
                Vector2 newPos = new Vector2(Position.X, newY);
                Position = newPos;
                Reflected = true;
            }

            //...the BOTTOM of the screen
            else if (Position.Y - EffectiveHeight > screenHeight)
            {
                float newY = GetReflectCoord(ScreenSide.Bottom, screenWidth, screenHeight);
                Vector2 newPos = new Vector2(Position.X, newY);
                Position = newPos;
                Reflected = true;
            }

        }

        /// <summary>
        /// Returns the reflected coordinate (x or y) based on what side of the screen the entity has passed.
        /// </summary>
        /// <param name="side">The side of the screen the entity has passed</param>
        /// <param name="screenWidth">The width of the screen</param>
        /// <param name="screenHeight">The height of the screen</param>
        /// <returns>The reflected x or y coordinate</returns>
        private float GetReflectCoord(ScreenSide side, float screenWidth, float screenHeight)
        {
            // Get the side, and return the corresponding x-coordinate
            switch (side)
            {
                //----------------
                // X Reflection
                case ScreenSide.Right:
                    return -EffectiveWidth;

                case ScreenSide.Left:
                    return (screenWidth + EffectiveWidth);
                //----------------

                //----------------
                // Y Reflection
                case ScreenSide.Top:
                    return (screenHeight + EffectiveHeight);

                case ScreenSide.Bottom:
                    return -EffectiveHeight;
                //----------------

                // If we got this far, something went horribly wrong
                default:
                    throw new ArgumentException("Invalid side", "side");
            }
        }
        

        #endregion
    }
}
