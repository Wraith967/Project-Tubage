using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    class NonPlayerEntity : DrawableEntity
    {
        #region Fields and Properties

        Rectangle drawbox;
        private Rectangle boundingBox;
        int height = 17;
        int[] indexedXWalkLeftRight = { 0, 15, 35, 55, 75 };
        int[] indexedXWalkUpDown = { 0, 14, 30, 44, 63, 77 };
        float posDelta = 2.0f;
        bool[] movement = { false, false, false, false }; // Down, Up, Left, Right

        public Rectangle getBoundary
        { get { return boundingBox; } }

        #endregion

        #region Initialization

        public NonPlayerEntity(string textureFileName, Vector2 pos)
            : base(textureFileName)
        {
            Position = pos;
            drawbox = new Rectangle(16, 63, 15, height);
            boundingBox = new Rectangle((int)Position.X, (int)Position.Y, 15, 11);
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void HandleInput(InputState input)
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
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch spriteBatch = OwnerScreen.ScreenManager.SpriteBatch;
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null);
            spriteBatch.Draw(Texture, position, drawbox, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }
        #endregion
    }
}
