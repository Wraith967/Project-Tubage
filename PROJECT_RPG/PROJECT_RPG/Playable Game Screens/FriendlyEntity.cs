using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    class FriendlyEntity : NonPlayerEntity
    {
        #region Fields and Properties
        
        float talkAgainTimer = 2000;
        bool canBeTalkedTo = true;
        String[][][] convo;
        protected String greeting;
        protected SpriteFont font;
        protected Vector2 greetingPos;

        #endregion

        #region Initialization

        public FriendlyEntity(string textureFileName, Vector2 pos, String greetingText, String convoFile)
            : base(textureFileName, pos)
        {
            convo = ConvoLoader.LoadConvo(convoFile);
            if (greetingText.Equals(""))
                greeting = "Hey! Talk to me! Please!";
            else
                greeting = greetingText;
            greetingPos = new Vector2(Position.X, Position.Y - 20f);
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
            if (!canBeTalkedTo)
            {
                talkAgainTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }
            if (talkAgainTimer < 0)
            {
                canBeTalkedTo = true;
                talkAgainTimer = 5000;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            Vector2 temp = Vector2.Subtract(greetingPos, Camera.Position);
            if (nearbyPlayer)
                spriteBatch.DrawString(font, greeting, temp, Color.White);
        }

        #endregion

        #region Public Methods

        public override void Interact(GameTime gameTime)
        {
            base.Interact(gameTime);
            if (canBeTalkedTo)
            {
                OwnerScreen.ScreenManager.AddScreen(new Conversation(convo));
                canBeTalkedTo = false;
            }
        }

        #endregion
    }
}
