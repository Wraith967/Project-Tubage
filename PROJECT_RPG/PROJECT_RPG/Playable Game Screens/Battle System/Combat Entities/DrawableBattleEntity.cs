using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    abstract class DrawableBattleEntity
    {
        #region Fields and Properties

        ContentManager content;

        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected BattleScreen owner;
        public BattleScreen OwnerScreen
        {
            get { return owner; }
            set { owner = value; }
        }

        protected Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        protected string textureFileName;
        public string TextureFileName
        {
            get { return textureFileName; }
            set { textureFileName = value; }
        }

        #endregion

        #region Initialization

        public DrawableBattleEntity(string textureFileName)
        {
            TextureFileName = textureFileName;
        }

        public virtual void LoadContent()
        {
            content = new ContentManager(OwnerScreen.ScreenManager.Game.Services, "Content");
            Texture = content.Load<Texture2D>(TextureFileName);
        }

        public virtual void UnloadContent() { }

        #endregion

        #region Update and Draw

        public virtual void Update(GameTime gameTime)
        { }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        #endregion
    }
}
