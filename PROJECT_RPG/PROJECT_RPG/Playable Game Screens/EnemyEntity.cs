using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    class EnemyEntity : NonPlayerEntity
    {
        #region Fields and Properties
        
        String battleFile;

        #endregion

        #region Initialization

        public EnemyEntity(string textureFileName, Vector2 pos, String greetingText, String battleFile)
            : base(textureFileName, pos, greetingText)
        {
            this.battleFile = battleFile;
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
            AudioManager.PauseSong();
            OwnerScreen.ScreenManager.AddScreen(new BattleScreen(battleFile, this, owner));
        }

        #endregion
    }
}
