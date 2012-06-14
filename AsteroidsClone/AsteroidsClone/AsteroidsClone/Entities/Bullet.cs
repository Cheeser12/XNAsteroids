#region Using Statements

using Microsoft.Xna.Framework;

#endregion

namespace AsteroidsClone
{
    /// <summary>
    /// Represents a bullet in the game
    /// </summary>
    public class Bullet : Entity
    {
        #region Variables/Properties

        /// <summary>
        /// The number of bullets currently active in the game.
        /// </summary>
        public static int NumBullets { get; private set; }

        /// <summary>
        /// Defines the lifespan of a bullet.
        /// </summary>
        private const float maxLifespan = 2f;

        /// <summary>
        /// Current lifespan of this bullet.
        /// </summary>
        private float currentLifespan = 0f;

        /// <summary>
        /// Shows whether the bullet has lived past its lifespan.
        /// </summary>
        public bool LifespanExceeded { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a bullet.
        /// </summary>
        /// <param name="pos">Initial position of the bullet.</param>
        /// <param name="dir">Direction the bullet is being fired to.</param>
        /// <param name="rot">Rotation of the bullet.</param>
        public Bullet(Vector2 pos, Vector2 dir, float rot)
            : base(new Rect(pos, 5f, 5f), rot, dir, 1.5f)
        {
            // Increment the number of active bullets.
            NumBullets++;

            LifespanExceeded = false;
            LinSpeed = 5f;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the bullet.
        /// </summary>
        /// <param name="gameTime">Timespan of the game</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Add the time passed to the current lifespan of the bullet.
            currentLifespan += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // If the bullet has exceeded its lifespan, note that and decrement the number of active bullets
            if (currentLifespan >= maxLifespan)
            {
                LifespanExceeded = true;
                NumBullets--;
            }
        }

        /// <summary>
        /// Determines if the bullet has hit another entity
        /// </summary>
        /// <param name="otherE">Entity to check against</param>
        /// <returns>Whether the bullet has hit otherE</returns>
        public bool HitEntity(Entity otherE)
        {
            return (Body.Intersects(otherE.Body));
        }

        #endregion
    }
}
