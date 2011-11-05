using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PROJECT_RPG
{
    class BattleUI
    {
        // General shit.
        SpriteFont font;

        // Screen that spawned this battle UI.
        BattleScreen battleScreen;
        public BattleScreen BattleScreen
        { get { return battleScreen; } }

        // Main RPG hud box thingy.
        RectangleOverlay mainHUD;

        string[] temp_Names = { "Bill", "Enemy"};
        Vector2 temp_NamesPos;
        string[] temp_Stats = { "30/30"};
        Vector2 temp_StatsPos;

        // Huehuehahuehauueha.
        public BattleUI(BattleScreen owner)
        {
            battleScreen = owner;

            mainHUD = new RectangleOverlay(new Rectangle(
                0,
                battleScreen.ScreenManager.GraphicsDevice.Viewport.Height - 100,
                battleScreen.ScreenManager.GraphicsDevice.Viewport.Width,
                100),
                battleScreen);
        }

        public void LoadContent()
        {
            font = battleScreen.ScreenManager.Font;

            // UI setup.
            mainHUD.LoadContent();
        }

        public void Update() { }

        public void Draw(SpriteBatch spriteBatch)
        {
            mainHUD.Draw();

            temp_NamesPos = new Vector2(mainHUD.GetInnerRectangle.X + 5, mainHUD.GetInnerRectangle.Y + 5);
            temp_StatsPos = new Vector2(mainHUD.GetInnerRectangle.X + 150, mainHUD.GetInnerRectangle.Y + 5);

            // Begin drawing.
            foreach (string s in temp_Names)
            {
                spriteBatch.DrawString(font, s, temp_NamesPos, Color.White, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0.0f);
                temp_NamesPos.Y += font.LineSpacing * 2.0f;
            }

            //spriteBatch.DrawString(font, BattleScreen.player.CurrentHP.ToString()+" / "+BattleScreen.player.MaxHP.ToString(), temp_StatsPos, Color.White, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0.0f);
            //temp_StatsPos.Y += font.LineSpacing * 2.0f;
            foreach (BattleScreenMember e in BattleScreen.BattleMembers){
                spriteBatch.DrawString(font, e.CurrentHP.ToString() + " / " + e.MaxHP.ToString(), temp_StatsPos, Color.White, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0.0f);
                temp_StatsPos.Y += font.LineSpacing * 2.0f;
            }

            // Done.
        }

        
    }
}
