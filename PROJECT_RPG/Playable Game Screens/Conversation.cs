using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    class Conversation : GameScreen
    {
        Vector2 location = new Vector2(250, 250);
        Vector2 textLocation;
        string[] conversation;
        int line = 0;
        RectangleOverlay box;

        public Conversation(string[] convo)
        {
            conversation = convo;
            TransitionOffTime = TimeSpan.Zero;
            TransitionOnTime = TimeSpan.Zero;
            IsPopup = true;
            
        }

        public override void LoadContent()
        {
            box = new RectangleOverlay(
                new Rectangle(
                    (int)location.X,
                    (int)location.Y,
                    (int)(200),
                    (75)
                    ), this);

            textLocation = new Vector2(
                box.GetInnerRectangle.X + 5,
                box.GetInnerRectangle.Y + 5);

            box.LoadContent();
        }

        public override void HandleInput(InputState input, GameTime gameTime)
        {
            if (input.IsMenuSelect() && (line != conversation.GetLength(0)))
            {
                line++;
                if (line == conversation.GetLength(0))
                {
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (input.IsMenuSelect() && (line == conversation.GetLength(0)))
            {
                ScreenManager.RemoveScreen(this);
            }
            
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            box.Draw();
            spriteBatch.DrawString(ScreenManager.Font, conversation[line], textLocation, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
