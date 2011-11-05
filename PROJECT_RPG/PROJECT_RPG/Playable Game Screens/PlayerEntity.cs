using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    class PlayerEntity : DrawableEntity
    {
        #region Fields and Properties

        int[] indexedXWalkLeftRight = { 0, 15, 35, 55, 75 }; // indexed animation x coordinates
        int[] indexedXWalkUpDown = { 0, 14, 30, 44, 63, 77 }; // indexed animation x coordinates
        int[] indexedY = { 0, 19, 39 }; // starting y coordinates for animations: left/right, down, up
        int indexX; // starting x coordinate of animation
        float posDelta = GlobalConstants.moveSpeed;
        bool[] movement = { false, false, false, false }; // Down, Up, Left, Right
        float animationUpdateTimer; // timer to update animation
        const float updateTime = 1000f / 15f; // base update time for animations
        int direction; // direction of animation, either 1 or -1

        #endregion

        #region Initialization

        public PlayerEntity(string textureFileName, Vector2 pos)
            : base(textureFileName)
        {
            Position = pos;
            height = 20;
            width = 15;
            drawbox = new Rectangle(16, 63, GetWidth, GetHeight);
            boundingBox = new Rectangle((int)Position.X, (int)Position.Y, 15, 16);
            animationUpdateTimer = updateTime;
            indexX = 0;
            direction = 1;
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            updateAnimation(gameTime);
        }

        private void updateAnimation(GameTime gameTime)
        {
            animationUpdateTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (animationUpdateTimer <= 0)
            {
                if (movement[0] || movement[1])
                {
                    indexX += direction;
                    if ((indexX == indexedXWalkUpDown.Length - 2) ||
                        (indexX == 0))
                        direction *= -1;
                    drawbox.X = indexedXWalkUpDown[indexX];
                    drawbox.Y = (movement[0] ? indexedY[1] : indexedY[2]);
                    drawbox.Width = indexedXWalkUpDown[indexX + 1] - indexedXWalkUpDown[indexX];
                }
                else if (movement[2] || movement[3])
                {
                    indexX += direction;
                    if ((indexX == indexedXWalkLeftRight.Length - 2) ||
                        (indexX == 0))
                        direction *= -1;
                    drawbox.X = indexedXWalkLeftRight[indexX];
                    drawbox.Y = indexedY[0];
                    drawbox.Width = indexedXWalkLeftRight[indexX + 1] - indexedXWalkLeftRight[indexX];
                }
                else
                {
                    indexX = 0;
                    direction = 1;
                    drawbox.X = indexedXWalkLeftRight[indexX];
                    drawbox.Y = indexedY[0];
                    drawbox.Width = indexedXWalkLeftRight[indexX + 1] - indexedXWalkLeftRight[indexX];
                }
                animationUpdateTimer += updateTime;
            }
        }

        public void HandleInput(InputState input, GameTime gameTime)
        {
            for (int i = 0; i < 4; i++)
                movement[i] = false;
            if (input.IsHoldDown())
            {
                position.Y += posDelta;
                movement[0] = true;
            }
            if (input.IsHoldUp())
            {
                position.Y -= posDelta;
                movement[1] = true;
            }
            if (input.IsHoldLeft())
            {
                position.X -= posDelta;
                movement[2] = true;
            }
            if (input.IsHoldRight())
            {
                position.X += posDelta;
                movement[3] = true;
            }
            boundingBox.X = (int)Position.X;
            boundingBox.Y = (int)Position.Y;
            if (input.IsUseButtonPressed())
                ((PlayableMainGameScreen)OwnerScreen).PlayerInteraction(gameTime);
        }

        public void undoMove()
        {
            Console.WriteLine("undoMove called");
            if (movement[0])
                position.Y -= posDelta;
            if (movement[1])
                position.Y += posDelta;
            if (movement[2])
                position.X += posDelta;
            if (movement[3])
                position.X -= posDelta;
            boundingBox.X = (int)Position.X;
            boundingBox.Y = (int)Position.Y;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
        #endregion
    }
}
