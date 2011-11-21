using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PROJECT_RPG
{
    class TitleScreen : GameScreen
    {
        #region Fields and Properties

        float timeToWaitBeforeExit = 7000;
        string titleString = "Down With The Interwebz!";

        #endregion

        #region Initialization

        // Bonjour, constructeur!
        public TitleScreen() 
        {
            TransitionOnTime = TimeSpan.FromSeconds(2);
            TransitionOffTime = TimeSpan.FromSeconds(2);
        }

        // Nothing to load or unload yet, but I imagine there will be so we add these for now.
        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // We want to display our title screen for a time...
            if (timeToWaitBeforeExit > 0)
            {
                timeToWaitBeforeExit -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            // ... then exit off after. Oh, and go to our menu screen.
            else
            {
                LoadingScreen.Load(screenManager, new MainMenuScreen());
            }
            
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Draw our title in a nice position on screen.
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 3);
            Vector2 titleOrigin = font.MeasureString(titleString) / 2;
            Color titleColor = new Color(255, 255, 255) * TransitionAlpha;
            float titleScale = 2.25f;

            spriteBatch.DrawString(font, titleString, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            

            spriteBatch.End();
        }


        #endregion

        #region Other Methods && Handle Input

        public override void HandleInput(InputState input, GameTime gameTime)
        { if (input.IsMenuCancel()) LoadingScreen.Load(screenManager, new MainMenuScreen()); }

        #endregion


    }
}
