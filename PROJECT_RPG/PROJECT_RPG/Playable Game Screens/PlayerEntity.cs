using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        Vector2 moveDir; // vector direction of movement
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
            moveDir = Vector2.Zero;
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
                if (!moveDir.Equals(Vector2.Zero))
                {
                    indexX += direction;
                    if ((indexX == (texture.Width / frameSize) - 1) ||
                        (indexX == 0))
                        direction *= -1;
                    if (moveDir.Y == posDelta)
                        indexY = 0;
                    else if (moveDir.Y == -posDelta)
                        indexY = 3;
                    else if (moveDir.X == -posDelta)
                        indexY = 1;
                    else if (moveDir.X == posDelta)
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
            moveDir = Vector2.Zero;
            if (input.IsHoldDown())
            {
                moveDir.Y = posDelta;
            }
            if (input.IsHoldUp())
            {
                moveDir.Y = -posDelta;
            }
            if (input.IsHoldLeft())
            {
                moveDir.X -= posDelta;
            }
            if (input.IsHoldRight())
            {
                moveDir.X += posDelta;
            }
            position += moveDir;
            boundingBox.X = (int)Position.X;
            boundingBox.Y = (int)Position.Y + 25;
            if (input.IsUseButtonPressed())
                ((PlayableMainGameScreen)OwnerScreen).PlayerInteraction(gameTime);
        }

        public void undoMove(int x, int y)
        {
            this.undoMove(new Vector2(x * GlobalConstants.tileSize, y * GlobalConstants.tileSize));
        }

        public void undoMove(Vector2 collision)
        {
            Vector2 temp = Vector2.Subtract(Position, collision);
            if ((temp.Y < -25) && (moveDir.Y > 0))
                position.Y -= posDelta;
            else if ((temp.Y > -25) && (moveDir.Y < 0))
                position.Y += posDelta;
            if ((temp.X > 20) && (moveDir.X < 0))
                position.X += posDelta;
            else if ((temp.X < 0) && (moveDir.X > 0))
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
