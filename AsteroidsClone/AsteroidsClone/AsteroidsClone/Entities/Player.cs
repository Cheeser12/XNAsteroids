#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace AsteroidsClone
{
    /// <summary>
    /// Represents the player, his ship, and the bullets he fires
    /// </summary>
    public class Player : Entity
    {
        #region Variables/Properties

        /// <summary>
        /// Current number of players
        /// </summary>
        public static int currentPlayers = 0;

        // Variables dealing with firing bullets
        private const float fireDelay = 0.5f;
        private float timeSinceFire = 0f;
        private bool fireDelayActive = false;

        // Physics vars
        private const float maxSpeed = 1.5f;
        private const float accel = 3f * 0.01f;
        private readonly float angSpeed;
        private const float frictionCoeff = 2f;

        /// <summary>
        /// A list of the bullets that have been fired by the player and are in play
        /// </summary>
        public List<Bullet> ActiveBullets { get; private set; }
        
        // Previous keyboard state
        KeyboardState prevKeyboardState;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates the player
        /// </summary>
        /// <param name="startPos">Start position</param>
        /// <param name="startRot">Start rotation</param>
        public Player(Vector2 startPos, float startRot) :
            base(new Triangle(new Vector2(0f, -0.5f), new Vector2(0.5f, 0.5f), new Vector2(-0.5f, 0.5f), startPos), startRot)
        {
            // If we have more than 1 player....well that's bad
            currentPlayers++;
            if (currentPlayers > 1)
                throw new InvalidOperationException("Max number of players exceeded.");

            // Initialize the angular speed 
            angSpeed = MathHelper.ToRadians(1f);

            Scale = 25f;
            

            ActiveBullets = new List<Bullet>();
            prevKeyboardState = Keyboard.GetState();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the player and all of his active bullets
        /// </summary>
        /// <param name="gameTime">Timespan of the game</param>
        public override void Update(GameTime gameTime)
        {
            // First get input from the user
            HandleInput(gameTime);

            // Find out what the new direction is.
            Direction = new Vector2((float)Math.Cos(Rotation + MathHelper.PiOver2), (float)Math.Sin(Rotation + MathHelper.PiOver2));
        
            base.Update(gameTime);

            // If the fire delay is active...
            if (fireDelayActive)
            {
                // ...add the elapsed seconds to the time since the player last fired
                timeSinceFire += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // If the time since the last bullet fire has exceeded fireDelay,
                // deactivate the delay and reset the time since last fire.
                if (timeSinceFire > fireDelay)
                {
                    fireDelayActive = false;
                    timeSinceFire = 0f;
                }
            }

            // Update the bullets
            for(int i = 0; i < ActiveBullets.Count; i++)
            {
                Bullet b = ActiveBullets[i];
                b.Update(gameTime);
                if (b.LifespanExceeded)
                    ActiveBullets.Remove(b);
            }

 
        }
        /// <summary>
        /// Handles user input
        /// </summary>
        /// <param name="gameTime">Timespan of the game</param>
        private void HandleInput(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            // We use playerMoved to determine if we should apply natural friction
            // If the player hasn't moved, we apply friction to slow them down
            // (I realize there's no friction in space, but hey....)
            bool playerMoved = false;

            // The following keys move the player linearly

            // Movement works by increasing the linear speed by an acceleration factor. The Entity base class takes it from there.

            // Each has a check inside of them: if the movement will keep the player below the maximum speed,
            // the movement is allowed. If the movement will move them above maximum speed, the speed stays capped.
            // Regardless, it will prevent natural friction by setting playerMoved to TRUE

            // Forward
            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                if (Math.Abs(LinSpeed - accel) < maxSpeed)
                    LinSpeed -= accel;

                playerMoved = true;
            }

            // Reverse
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                if (Math.Abs(LinSpeed + accel) < maxSpeed)
                    LinSpeed += accel;
                playerMoved = true;
            }

            // Apply friction? If the player hasn't moved but the ship still has momentum, we apply natural friction to slow it down.
            if (!playerMoved && Math.Abs(LinSpeed) > 0)
            {
                if (LinSpeed > 0f)
                    LinSpeed -= frictionCoeff * 0.01f;

                else
                    LinSpeed += frictionCoeff * 0.01f;
            }

            // Rotation
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                Rotation -= angSpeed;
                
            }

            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                Rotation += angSpeed;          
            }

            // Actions

            // Fires a bullet.
            // Checks to see if the fireDelay is active as well
            if (currentKeyboardState.IsKeyDown(Keys.Space) && !fireDelayActive)
            {
                ActiveBullets.Add(new Bullet(Position, -Direction, Rotation));
                fireDelayActive = true;
            }


            prevKeyboardState = currentKeyboardState;
                
        }

        /// <summary>
        /// Reflects the player across the screen.
        /// </summary>
        /// <param name="screenWidth">Width of the screen</param>
        /// <param name="screenHeight">Height of the screen</param>
        public override void ReflectAcrossScreen(float screenWidth, float screenHeight)
        {
            base.ReflectAcrossScreen(screenWidth, screenHeight);
            foreach (Bullet b in ActiveBullets)
            {
                b.ReflectAcrossScreen(screenWidth, screenHeight);
            }
        }

        /// <summary>
        /// Draws the player and his bullets
        /// </summary>
        /// <param name="rend">Renderer for drawing</param>
        public override void Draw(Renderer rend)
        {
            base.Draw(rend);
            foreach (Bullet b in ActiveBullets)
            {
                b.Draw(rend);
            }
        }

        #endregion
    }
}


