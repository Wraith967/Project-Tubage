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

        //Rectangle drawbox;
        //private Rectangle boundingBox;
        //protected int[] indexedXWalkLeftRight = { 0, 15, 35, 55, 75 };
        //protected int[] indexedXWalkUpDown = { 0, 14, 30, 44, 63, 77 };
        //float posDelta = 2.0f;
        //bool[] movement = { false, false, false, false }; // Down, Up, Left, Right
        bool nearbyPlayer = false;
        String greeting;
        protected SpriteFont font;
        Vector2 greetingPos;
        int playerDist;
        Random gen;

        

        #endregion

        #region Initialization

        public NonPlayerEntity(string textureFileName, Vector2 pos, String greetingText)
            : base(textureFileName)
        {
            Position = pos;
            width = 15;
            height = 17;
            drawbox = new Rectangle(0, 0, GetWidth, GetHeight);
            boundingBox = new Rectangle((int)Position.X, (int)Position.Y, 15, 11);
            if (greetingText.Equals(""))
                greeting = "Hey! Talk to me! Please!";
            else
                greeting = greetingText;
            greetingPos = new Vector2(pos.X, pos.Y - 20f);
            gen = new Random(DateTime.Now.Millisecond);
            playerDist = gen.Next(20, 100);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            font = OwnerScreen.ScreenManager.Font;
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            NearbyPlayer(((PlayableMainGameScreen)owner).Player.Position);
        }

        public bool HasCollision()
        {
            bool collision = boundingBox.Intersects(((PlayableMainGameScreen)owner).Player.getBoundary);
            return collision;
        }

        public void NearbyPlayer(Vector2 playerPos)
        {
            float diff = (Position - playerPos).Length();
            if (diff < playerDist)
            {
                nearbyPlayer = true;
            }
            else
                nearbyPlayer = false;
        }        

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            if (nearbyPlayer)
                spriteBatch.DrawString(font, greeting, greetingPos, Color.White);
        }
        #endregion

        #region Public Methods

        public virtual void Interact(GameTime gameTime)
        {
        }

        #endregion
    }
}
