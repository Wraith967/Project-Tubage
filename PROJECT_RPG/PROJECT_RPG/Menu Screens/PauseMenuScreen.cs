using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PROJECT_RPG
{
    class PauseScreen : GameScreen
    {
        public PauseScreen()
        {
            transitionOnTime = TimeSpan.FromSeconds(0);
            TransitionOffTime = TimeSpan.FromSeconds(0);
            IsPopup = true;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            Vector2 position = new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2);
            Vector2 origin = font.MeasureString("Paused") / 2;
            Color titleColor = new Color(255, 255, 255) * TransitionAlpha;
            float scale = 2.0f;

            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Paused", position, Color.White, 0.0f, origin, scale, SpriteEffects.None, 0.0f);
            //spriteBatch.DrawString(font, ScreenManager.GetScreens().Length.ToString(), new Vector2(256, 256), Color.Red);

            spriteBatch.End();
        }

        public override void HandleInput(InputState input, GameTime gameTime)
        {
            base.HandleInput(input, gameTime);
            if (input.IsPauseButtonPressed()) { ExitScreen(); }
        }
    }
}
