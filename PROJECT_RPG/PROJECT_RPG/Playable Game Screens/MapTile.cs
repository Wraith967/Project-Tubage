using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PROJECT_RPG
{
    class MapTile
    {
        #region Fields and Properties

        private Rectangle drawbox;
        private Rectangle boundaryBox;
        public Rectangle getBoundary
        { get { return boundaryBox; } }

        const int tileHeightInPixels = 20;
        public int GetHeight
        { get { return tileHeightInPixels; } }

        const int tileWidthInPixels = 20;
        public int GetWidth
        { get { return tileWidthInPixels; } }

        private Vector2 position;
        public Vector2 Position
        { 
            get { return position; }
            set { position = value; }
        }

        private PlayableMainGameScreen owner;

        private String textureKey;

        private Texture2D texture;
        public Texture2D Texture
        { get { return texture; } }

        private MapTileCollisionType collision;
        public MapTileCollisionType Collision
        { get { return collision; } }

        private Vector2 transferPoint;
        public Vector2 TransferPoint
        {
            get { return transferPoint; }
            set { transferPoint = value; }
        }

        private String nextMapFile;
        public String NextMapFile
        {
            get { return nextMapFile; }
            set { nextMapFile = value; }
        }

        private bool isTransferPoint;
        public bool IsTransfer
        {
            get { return isTransferPoint; }
            set { isTransferPoint = value; }
        }

        #endregion

        #region Initialization

        public MapTile(String textureKey, Vector2 pos, MapTileCollisionType collision, PlayableMainGameScreen owner)
        {
            this.textureKey = textureKey;
            position = pos;
            this.collision = collision;
            this.owner = owner;
            this.drawbox = new Rectangle((int)pos.X, (int)pos.Y, GetWidth, GetHeight);
            CreateBoundary();
            
        }

        private void CreateBoundary()
        {
            if (collision == MapTileCollisionType.FullCollision)
                boundaryBox = new Rectangle((int)position.X, (int)position.Y, GetWidth, GetHeight);
            else if (collision == MapTileCollisionType.HalfCollisionBot)
                boundaryBox = new Rectangle((int)position.X, (int)position.Y, GetWidth, GetHeight / 2);
            else if (collision == MapTileCollisionType.HalfCollisionLeft)
                boundaryBox = new Rectangle((int)position.X + GetWidth / 2, (int)position.Y, GetWidth / 2, GetHeight);
            else if (collision == MapTileCollisionType.HalfCollisionRight)
                boundaryBox = new Rectangle((int)position.X, (int)position.Y, GetWidth / 2, GetHeight);
            else if (collision == MapTileCollisionType.HalfCollisionTop)
                boundaryBox = new Rectangle((int)position.X, (int)position.Y + GetHeight / 2, GetWidth, GetHeight / 2);
            else
                boundaryBox = new Rectangle((int)position.X, (int)position.Y, 0, 0);
        }

        public void LoadContent()
        {
            texture = owner.ScreenManager.MapTex.getTexture(textureKey);
        }


        #endregion

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            boundaryBox.X = (int)Position.X;
            boundaryBox.Y = (int)Position.Y;
        }

        public bool CollisionCheck(Rectangle rect)
        {
            //if (collision == MapTileCollisionType.HalfCollisionLeft)
            //{
            //    LoadingScreen.Load(owner.ScreenManager, new PlayableMainGameScreen("testTileMap2.txt", new Vector2(60, 265)));
            //}
            //else if (collision == MapTileCollisionType.HalfCollisionRight)
            //{
            //    LoadingScreen.Load(owner.ScreenManager, new PlayableMainGameScreen("testTileMap.txt", new Vector2(490, 265)));
            //}
            //return rect.Intersects(boundaryBox);
            bool collide = rect.Intersects(boundaryBox);

            if (IsTransfer)
            {
                LoadingScreen.Load(owner.ScreenManager, new PlayableMainGameScreen(nextMapFile, transferPoint));
            }

            return collide;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 tempPosition = Vector2.Subtract(Position, Camera.Position);
            drawbox.X = (int)tempPosition.X;
            drawbox.Y = (int)tempPosition.Y;
            // Why do we make a new rectangle every draw call? :[
            spriteBatch.Draw(texture, drawbox, Color.White);
        }

        #endregion

        #region Public Methods

        public void UpdatePosition(int direction)
        {
            switch (direction)
            {
                case 0:
                    position.Y += GetHeight;
                    break;
                case 1:
                    position.X -= GetWidth;
                    break;
                case 2:
                    position.Y -= GetHeight;
                    break;
                case 3:
                    position.X += GetWidth;
                    break;
            }
        }

        #endregion
    }
}
