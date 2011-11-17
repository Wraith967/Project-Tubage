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

        int frameSize; // frame coordinates determined by frameSize * index
        int indexX; // x index of frame
        int indexY; // y index of frame
        float posDelta = GlobalConstants.moveSpeed;
        bool[] movement = { false, false, false, false, true }; // Down, Up, Left, Right, Idle
        float animationUpdateTimer; // timer to update animation
        const float updateTime = 1000f / 15f; // base update time for animations
        int direction; // direction of animation, either 1 or -1

        #endregion

        #region Initialization

        public PlayerEntity(string textureFileName, Vector2 pos)
            : base(textureFileName, pos)
        {
            indexX = 0;
            indexY = 0;
            animationUpdateTimer = updateTime;
            direction = 1;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            frameSize = texture.Width / 3;
            height = frameSize;
            width = frameSize;
            drawbox = new Rectangle(indexX * frameSize, indexY * frameSize, GetWidth, GetHeight);
            boundingBox = new Rectangle((int)Position.X, (int)Position.Y + 25, 40, 15);
            if (drawbox.Width != 40)
            {
                scale = 40.0f / drawbox.Width;
            }
            else
            {
                scale = 1.0f;
            }
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
                if (!movement[4])
                {
                    indexX += direction;
                    if ((indexX == (texture.Width / frameSize) - 1) ||
                        (indexX == 0))
                        direction *= -1;
                    if (movement[0])
                        indexY = 0;
                    else if (movement[1])
                        indexY = 3;
                    else if (movement[2])
                        indexY = 1;
                    else if (movement[3])
                        indexY = 2;
                    else
                        indexY = 0;
                }
                else
                {
                    indexX = 0;
                    indexY = 0;
                    direction = 1;
                }
                drawbox.X = indexX * frameSize;
                drawbox.Y = indexY * frameSize;
                animationUpdateTimer += updateTime;
            }
        }

        public void HandleInput(InputState input, GameTime gameTime)
        {
            for (int i = 0; i < 4; i++)
                movement[i] = false;
            movement[4] = true;
            if (input.IsHoldDown())
            {
                position.Y += posDelta;
                movement[0] = true;
                movement[4] = false;
            }
            if (input.IsHoldUp())
            {
                position.Y -= posDelta;
                movement[1] = true;
                movement[4] = false;
            }
            if (input.IsHoldLeft())
            {
                position.X -= posDelta;
                movement[2] = true;
                movement[4] = false;
            }
            if (input.IsHoldRight())
            {
                position.X += posDelta;
                movement[3] = true;
                movement[4] = false;
            }
            boundingBox.X = (int)Position.X;
            boundingBox.Y = (int)Position.Y + 25;
            if (input.IsUseButtonPressed())
                ((PlayableMainGameScreen)OwnerScreen).PlayerInteraction(gameTime);
        }

        public void undoMove()
        {
            if (movement[0])
                position.Y -= posDelta;
            if (movement[1])
                position.Y += posDelta;
            if (movement[2])
                position.X += posDelta;
            if (movement[3])
                position.X -= posDelta;
            boundingBox.X = (int)Position.X;
            boundingBox.Y = (int)Position.Y + 25;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
        #endregion
    }
}
