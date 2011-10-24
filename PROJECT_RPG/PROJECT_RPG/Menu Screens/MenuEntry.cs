using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    class MenuEntry
    {

        #region Fields and Properties

        Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; } }

        bool pulse = true;
        public bool IsPulseMenuEntry { get { return pulse; } set { pulse = value; } }

        float selectionFade;

        float scaleFactor = 2f;
        public float ScaleFactor
        { get { return scaleFactor; } set { scaleFactor = value; } }

        string text;
        public string Text { get { return text; } set { text = value; } }

        #endregion

        #region Initialization

        // Oh hello, Sir Constructor!
        public MenuEntry(string text)
        {
            this.text = text;
        }

        #endregion

        #region Menu Entry Selection - Event


        // Because we needs the precious.
        public event EventHandler<EventArgs> Selected;

        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
            { Selected(this, new EventArgs());}
        }

        #endregion

        #region Update and Draw

        public virtual void Update(GameScreen screen, bool isSelected, GameTime gameTime) // USED TO BE MENUSCREEN
        {
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly switching between states.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }

        public virtual void Draw(GameScreen screen, bool isSelected, GameTime gameTime) // USED TO BE MENUSCREEN
        {
            // To do: change from pulsing to perhaps an arrow icon alongside, or other fancy stuff.

            // Draw the selected entry in yellow, otherwise white.
            Color color = isSelected ? Color.Yellow : Color.White;

            // Pulsate the size of the selected menu entry.
            // Special thanks to Jeff for the maff.
            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale;
            if (IsPulseMenuEntry)
            {
                scale = scaleFactor + pulsate * 0.05f * selectionFade;
            }
            else { scale = scaleFactor; }

            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, color, 0,
                                   origin, scale, SpriteEffects.None, 0);
        }

        // Gets width of the string based on the font. 
        public virtual int GetWidth(GameScreen screen) // USED TO BE MENU SCREEN
        {
            return (int)(screen.ScreenManager.Font.MeasureString(Text).X * scaleFactor);
        }

        // Gets height of the string based on the font.
        public virtual int GetHeight(GameScreen screen) // USED TO BE MENU SCREEN
        {
            return (int)(screen.ScreenManager.Font.LineSpacing * scaleFactor);
        }

        #endregion

        
    }
}
