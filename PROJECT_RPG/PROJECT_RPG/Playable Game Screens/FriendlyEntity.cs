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
        String[] convo;

        #endregion

        #region Initialization

        public FriendlyEntity(string textureFileName, Vector2 pos, String greetingText, String convoFile)
            : base(textureFileName, pos, greetingText)
        {
            convo = ConvoLoader.LoadConvo(convoFile);
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
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

        #endregion
    }
}
