using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PROJECT_RPG
{
    // Probably should be reused for everything!
    class RectangleOverlay
    {
        SpriteBatch spriteBatch;
        Texture2D texture;
        GameScreen owner;

        Rectangle outerRectangle;
        public Rectangle GetOuterRectangle
        { get { return outerRectangle; } }

        Rectangle innerRectangle;
        public Rectangle GetInnerRectangle
        { get { return innerRectangle; } }

        Color boxColor;
        Color borderColor;

        public RectangleOverlay(Rectangle r, GameScreen o)
        {
            outerRectangle = r;
            innerRectangle = new Rectangle((int)r.X + 2, (int)r.Y + 2, r.Width - 4, r.Height - 4);
            owner = o;
            boxColor = Color.DarkBlue;
            borderColor = Color.White;
        }

        public void LoadContent()
        {
            spriteBatch = owner.ScreenManager.SpriteBatch;
            texture = new Texture2D(owner.ScreenManager.SpriteBatch.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
        }

        public void Draw()
        {
            spriteBatch.Draw(texture, outerRectangle, borderColor);
            spriteBatch.Draw(texture, innerRectangle, boxColor);
        }

    }
}
