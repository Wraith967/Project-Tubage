using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    class Blank2EntryScreen : GameScreen
    {
        #region Fields and Properties

        bool isActiveEntryScreen;
        public bool IsActiveEntryScreen
        { get { return isActiveEntryScreen; } set { isActiveEntryScreen = value; } }

        List<MenuEntry> menuEntires = new List<MenuEntry>();
        public List<MenuEntry> MenuEntries
        { get { return menuEntires; } set { menuEntires = value; } }

        int selectedMajorMenuEntry = 0;
        public int SelectedMajorMenuEntry
        { get { return selectedMajorMenuEntry; } set { selectedMajorMenuEntry = value; } }

        int topDisplayedEntryIndex;
        int botDisplayedEntryIndex;

        Vector2 scrollUp = new Vector2(25,70);
        Vector2 scrollDown = new Vector2(25,135);


        #endregion

        #region Initialization

        public Blank2EntryScreen()
        {
            TransitionOffTime = TimeSpan.Zero;
            TransitionOnTime = TimeSpan.Zero;
            IsPopup = true;

            // Menu entry stuff.
            MenuEntry testEntry = new MenuEntry("test");
            MenuEntry testEntry2 = new MenuEntry("test1");
            MenuEntry testEntry3 = new MenuEntry("test2");
            MenuEntry testEntry4 = new MenuEntry("test3");
            MenuEntry testEntry5 = new MenuEntry("test4");
            MenuEntry testEntry6 = new MenuEntry("test5");
            MenuEntry testEntry7 = new MenuEntry("test6");
            MenuEntry testEntry8 = new MenuEntry("test7");
            MenuEntries.Add(testEntry);
            MenuEntries.Add(testEntry2);
            MenuEntries.Add(testEntry3);
            MenuEntries.Add(testEntry4);
            MenuEntries.Add(testEntry5);
            MenuEntries.Add(testEntry6);
            MenuEntries.Add(testEntry7);
            MenuEntries.Add(testEntry8);
            
            foreach (MenuEntry entry in MenuEntries)
            { entry.IsPulseMenuEntry = false; entry.ScaleFactor = 1.15f; }

            botDisplayedEntryIndex = 3;
            topDisplayedEntryIndex = 0;
        }

        public override void LoadContent()
        {
            //scrollUp.Y -= ScreenManager.Font.LineSpacing / 2;
            scrollUp.X -= ScreenManager.Font.MeasureString("}").X / 2;
            scrollDown.X += ScreenManager.Font.MeasureString("}").X / 2;
        }
        #endregion

        #region Update and Draw
        public override void HandleInput(InputState input, GameTime gameTime)
        {
            // Move to the previous menu entry?
            if (input.IsKeyUp() || input.IsKeyRight())
            {
                selectedMajorMenuEntry--;
                screenManager.MenuItemSound.Play(0.5f, 0.0f, 0.0f);
                if (selectedMajorMenuEntry < 0) { selectedMajorMenuEntry++; }
                else if (selectedMajorMenuEntry < topDisplayedEntryIndex) { topDisplayedEntryIndex--; botDisplayedEntryIndex--; }
            }

            // Move to the next menu entry?
            if (input.IsKeyDown() || input.IsKeyLeft())
            {
                selectedMajorMenuEntry++;
                screenManager.MenuItemSound.Play(0.5f, 0.0f, 0.0f);
                if (selectedMajorMenuEntry > MenuEntries.Count - 1) { selectedMajorMenuEntry--; }
                else if (selectedMajorMenuEntry > botDisplayedEntryIndex) { topDisplayedEntryIndex++; botDisplayedEntryIndex++; }
            }

            // Handle selecting menu entries to proceed to next screen, or exiting from this screen.
            if (input.IsMenuSelect())
            {
            }
            else if (input.IsMenuCancel())
            {
                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Draw each menu entry in turn.
            for (int i = topDisplayedEntryIndex; i <= botDisplayedEntryIndex; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];

                bool isSelected = IsActive && (i == SelectedMajorMenuEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate * 0.05f; //* selectionFade;

            if (topDisplayedEntryIndex != 0)
            {
                spriteBatch.DrawString(font, "}", scrollUp, Color.White, -1.5f, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
            if (botDisplayedEntryIndex != MenuEntries.Count - 1)
            {
                spriteBatch.DrawString(font, "}", scrollDown, Color.White, 1.5f, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
            spriteBatch.End();
            
        }

        protected void UpdateMenuEntryLocations()
        {
            // Beginning position of the menu entries.
            Vector2 position = new Vector2(10f, 85f);

            // update each menu entry's location in turn
            for (int i = topDisplayedEntryIndex; i <= botDisplayedEntryIndex; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight(this);
            }
        }
        #endregion
    }
}
