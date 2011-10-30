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

        float talkAgainTimer = 2000;
        bool canBeTalkedTo = true;
        //Rectangle drawbox;
        //private Rectangle boundingBox;
        int[] indexedXWalkLeftRight = { 0, 15, 35, 55, 75 };
        int[] indexedXWalkUpDown = { 0, 14, 30, 44, 63, 77 };
        //float posDelta = 2.0f;
        //bool[] movement = { false, false, false, false }; // Down, Up, Left, Right
        bool nearbyPlayer = false;
        String greeting;
        SpriteFont font;
        Vector2 greetingPos;

        

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
            if (nearbyPlayer && canBeTalkedTo)
            {
                OwnerScreen.ScreenManager.AddScreen(new Conversation(new string[] { greeting }));
                canBeTalkedTo = false;
            }
            else if (!canBeTalkedTo)
            {
                talkAgainTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }
            if (talkAgainTimer < 0)
            {
                canBeTalkedTo = true;
                talkAgainTimer = 5000;
            }
        }

        public bool HasCollision()
        {
            bool collision = boundingBox.Intersects(((PlayableMainGameScreen)owner).Player.getBoundary);
            return collision;
        }

        public void NearbyPlayer(Vector2 playerPos)
        {
            float diff = (Position - playerPos).Length();
            if (diff < 40)
            {
                nearbyPlayer = true;
            }
            else
                nearbyPlayer = false;
        }        

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
        #endregion
    }
}
