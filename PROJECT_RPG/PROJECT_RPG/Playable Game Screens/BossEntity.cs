using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PROJECT_RPG
{
    class BossEntity : EnemyEntity
    {
        #region Fields and Properties
        
        float talkAgainTimer = 100;
        bool canBeTalkedTo = true;
        String[] convo;
        bool canFight = false;

        #endregion

        #region Initialization

        public BossEntity(string textureFileName, Vector2 pos, String convoFile, String battleFile)
            : base(textureFileName, pos, battleFile)
        {
            convo = ConvoLoader.LoadConvo(convoFile);
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (nearbyPlayer)
                Interact(gameTime);
            if (!canBeTalkedTo)
            {
                talkAgainTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }
            if (talkAgainTimer < 0)
            {
                canFight = true;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        #endregion

        #region Public Methods

        public override void Interact(GameTime gameTime)
        {
            if (canBeTalkedTo)
            {
                OwnerScreen.ScreenManager.AddScreen(new Conversation(convo));
                canBeTalkedTo = false;
            }
            if (canFight)
            {
                base.Interact(gameTime);
            }
        }

        #endregion
    }
}
