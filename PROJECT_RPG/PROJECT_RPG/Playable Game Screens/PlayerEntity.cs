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

        //Rectangle drawbox;
        //private Rectangle boundingBox;
        int[] indexedXWalkLeftRight = { 0, 15, 35, 55, 75 };
        int[] indexedXWalkUpDown = { 0, 14, 30, 44, 63, 77 };
        float posDelta = GlobalConstants.moveSpeed;
        bool[] movement = { false, false, false, false }; // Down, Up, Left, Right
        Vector2 lastPosition;

        #endregion

        #region Initialization

        public PlayerEntity(string textureFileName, Vector2 pos)
            : base(textureFileName)
        {
            Position = pos;
            height = 17;
            width = 15;
            drawbox = new Rectangle(16, 63, GetWidth, GetHeight);
            boundingBox = new Rectangle((int)Position.X, (int)Position.Y, 15, 11);
            lastPosition = Camera.Position;
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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
            if (!lastPosition.Equals(Camera.Position))
            {
                //Position = Vector2.Subtract(Position, Camera.Position);
                lastPosition = Camera.Position;
            }
            //spriteBatch.Draw(Texture, Position, drawbox, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            base.Draw(gameTime, spriteBatch);
        }
        #endregion
    }
}
