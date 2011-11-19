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
    class StatBar
    {
        SpriteBatch spriteBatch;
        Texture2D texture;
        GameScreen owner;
        bool energy = true;

        Rectangle outerRectangle;
        public Rectangle GetOuterRectangle
        { get { return outerRectangle; } }

        Rectangle innerRectangle;
        public Rectangle GetInnerRectangle
        { get { return innerRectangle; } }

        Color boxColor;
        Color borderColor;

        /// <summary>
        /// Constructor!
        /// </summary>
        /// <param name="r"></param>
        /// <param name="o"></param>
        /// <param name="energyOrExp">true = energy, false = exp</param>
        public StatBar(Rectangle r, GameScreen o, bool energyOrExp)
        {
            outerRectangle = r;
            innerRectangle = new Rectangle((int)r.X + 2, (int)r.Y + 2, r.Width - 4, r.Height - 4);
            if (energy)
            {
                innerRectangle.Width = (int)(innerRectangle.Width * ((float)PlayerStats.CurrentEnergy / PlayerStats.MaximumEnergy));
            }
            else innerRectangle.Width = (int)(innerRectangle.Width * ((float)PlayerStats.CurrentXP / PlayerStats.NextLevelXP));
            owner = o;
            energy = energyOrExp;
            if (energy)
            {
                boxColor = Color.Red;
            }
            else boxColor = Color.Green;
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
