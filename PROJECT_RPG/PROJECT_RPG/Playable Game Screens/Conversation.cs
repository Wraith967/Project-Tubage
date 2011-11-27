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
        Vector2 textLocation;
        string[][] conversation;
        string[][] conversation_options;
        int line = 0;
        RectangleOverlay box;
        float textScale = 1.5f;
        bool IsOptions = false;

        string tag_text = "text";
        string tag_opti = "option";

        int selectedOption = 1;

        public Conversation(string[][][] convo)
        {
            conversation = convo[0];
            conversation_options = convo[1];
            TransitionOffTime = TimeSpan.Zero;
            TransitionOnTime = TimeSpan.Zero;
            IsPopup = true;
        }

        public override void LoadContent()
        {
            box = new RectangleOverlay(
                new Rectangle(
                    0,
                (ScreenManager.GraphicsDevice.Viewport.Height - 100),
                (ScreenManager.GraphicsDevice.Viewport.Width),
                100),
                this);

            textLocation = new Vector2(
                box.GetInnerRectangle.X + 5,
                box.GetInnerRectangle.Y + 5);

            box.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            //base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (!IsOptions)
            {
                if (conversation[line][0] == tag_opti)
                    IsOptions = true;
            }
            else
            {
                if (conversation[line][0] == tag_text)
                    IsOptions = false;
            }
        }

        public override void HandleInput(InputState input, GameTime gameTime)
        {
            if (!IsOptions)
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
            else if (IsOptions)
            {
                if (input.IsKeyLeft() || input.IsKeyDown())
                {
                    selectedOption--;
                    if (selectedOption < 1)
                        selectedOption = conversation_options.GetLength(0) - 1;
                }
                else if (input.IsKeyRight() || input.IsKeyUp())
                {
                    selectedOption++;
                    if (selectedOption == conversation_options.GetLength(0))
                        selectedOption = 1;
                }
                else if (input.IsMenuSelect())
                {
                    HandleOption(conversation_options[selectedOption][2]);
                    IsOptions = false;
                    line++;
                }
            }
        }

        void HandleOption(string option)
        {
            char[] delims = { '=' };
            String[] tokens = option.Substring(0).Split(delims);
            switch (tokens[0])
            {
                case "warp":
                    Warp(tokens[1], Int32.Parse(tokens[2]), Int32.Parse(tokens[3]));
                    break;
                case "none":
                    break;
                default:
                    break;
            }
        }

        void Warp(string mapname, int transferPointX, int transferPointY)
        {
            ScreenManager.AddScreen(new PlayableMainGameScreen(mapname, new Vector2(transferPointX, transferPointY)));
        }

        // word wrapper -- should take into account scaling automatically
        String WrapText(String text)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (ScreenManager.Font.MeasureString(line + word).Length() * textScale > box.GetInnerRectangle.Width)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            box.Draw();

            textLocation.Y = box.GetInnerRectangle.Y + 5;
            textLocation.X = box.GetInnerRectangle.X + 5;
            if (conversation[line][0] == tag_text)
            {
                spriteBatch.DrawString(ScreenManager.Font, WrapText(conversation[line][1]), textLocation, Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
            }
            else if (conversation[line][0] == tag_opti)
            {
                spriteBatch.DrawString(ScreenManager.Font, WrapText(conversation_options[0][1]), textLocation, Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
                textLocation.Y += ScreenManager.Font.LineSpacing * textScale;
                textLocation.X += 25;
                for (int i = 1; i < conversation_options.GetLength(0); i++)
                {
                    spriteBatch.DrawString(ScreenManager.Font, WrapText(conversation_options[i][1]), textLocation, Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
                    if (i == selectedOption)
                    {
                        textLocation.X = box.GetInnerRectangle.X + 5;
                        spriteBatch.DrawString(ScreenManager.Font, "}", textLocation, Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0);
                        textLocation.X += 25;
                    }
                    textLocation.Y += ScreenManager.Font.LineSpacing * textScale;
                }
            }
            spriteBatch.End();
        }
    }
}
