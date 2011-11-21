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
        protected bool nearbyPlayer = false;
        int playerDist;
        Random gen;

        #endregion

        #region Initialization

        public NonPlayerEntity(string textureFileName, Vector2 pos)
            : base(textureFileName, pos)
        {
            gen = new Random(DateTime.Now.Millisecond);
            playerDist = gen.Next(20, 100);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            width = texture.Width;
            height = texture.Height;
            drawbox = new Rectangle(0, 0, GetWidth, GetHeight);
            boundingBox = new Rectangle((int)Position.X, (int)Position.Y, GetWidth, GetHeight);
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
        }
        #endregion

        #region Public Methods

        public virtual void Interact(GameTime gameTime)
        {
        }

        #endregion
    }
}
