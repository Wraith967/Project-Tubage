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
        String battleFile;

        public EnemyEntity(string textureFileName, Vector2 pos, String greetingText, String battleFile)
            : base(textureFileName, pos, greetingText)
        {
            this.battleFile = battleFile;
        }

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
    }
}
