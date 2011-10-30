using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PROJECT_RPG
{
    abstract class DrawableEntity
    {
        #region Fields and Properties

        protected ContentManager content;
        protected Rectangle drawbox;
        protected Rectangle boundingBox;

        public Rectangle getBoundary
        { get { return boundingBox; } }

        protected int height;
        public int GetHeight
        {
            get { return height; }
            set { height = value; }
        }

        protected int width;
        public int GetWidth
        {
            get { return width; }
            set { width = value; }
        }

        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                boundingBox.X = (int)position.X;
                boundingBox.Y = (int)position.Y;
            }
        }

        protected GameScreen owner;
        public GameScreen OwnerScreen
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

        public DrawableEntity(string textureFileName)
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
        {
            Vector2 tempPosition = Vector2.Subtract(position, Camera.Position);
            spriteBatch.Draw(Texture, tempPosition, drawbox, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }

        #endregion
    }
}
