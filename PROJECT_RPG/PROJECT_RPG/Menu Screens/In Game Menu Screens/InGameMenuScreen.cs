using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    class InGameMenuScreen : GameScreen
    {
        // This whole damn screen is a placeholder.
        // General in game menu stuff.
        // Mostly placeholders!
        int currentMonies = 1184889;
        Vector2 moniesPosition = new Vector2(0, 15);
        int currentXP = 14;
        int nextLevelXP = 249;
        Vector2 xpPosition = new Vector2(10, 15);

        // Major menu 1 stuff.
        List<MenuEntry> menuEntries = new List<MenuEntry>();
        public IList<MenuEntry> MenuEntries
        { get { return menuEntries; } }

        int selectedMajorMenuEntry = 0;
        public int SelectedMajorMenuEntry
        { get { return selectedMajorMenuEntry; } set { selectedMajorMenuEntry = value; } }

        // Entry screens

        public InGameMenuScreen()
        {
            TransitionOnTime = TimeSpan.Zero;
            TransitionOffTime = TimeSpan.Zero;

            MenuEntry MajorEntry1 = new MenuEntry("MajorEntry1"); MajorEntry1.Selected += MajorEntry1Selected;
            MenuEntry MajorEntry2 = new MenuEntry("MajorEntry2"); MajorEntry2.Selected += MajorEntry2Selected;
            MenuEntry MajorEntry3 = new MenuEntry("MajorEntry3"); MajorEntry3.Selected += MajorEntry3Selected;
            MenuEntry MajorEntry4 = new MenuEntry("MajorEntry4"); MajorEntry4.Selected += MajorEntry4Selected;
            MenuEntry MajorEntry5 = new MenuEntry("MajorEntry5"); MajorEntry5.Selected += MajorEntry5Selected;

            menuEntries.Add(MajorEntry1);
            menuEntries.Add(MajorEntry2);
            menuEntries.Add(MajorEntry3);
            menuEntries.Add(MajorEntry4);
            menuEntries.Add(MajorEntry5);

            // Differentiate menu entries in this menu from main menu entries.
            foreach (MenuEntry entry in menuEntries)
            { entry.IsPulseMenuEntry = false; entry.ScaleFactor = 1.25f; }
        }

        #region Handle Input Events
        // Handle input events.
        void MajorEntry1Selected(object sender, EventArgs e)
        { screenManager.AddScreen(new BlankEntryScreen()); }
        void MajorEntry2Selected(object sender, EventArgs e)
        { screenManager.AddScreen(new Blank2EntryScreen()); }
        void MajorEntry3Selected(object sender, EventArgs e)
        { screenManager.AddScreen(new Blank3EntryScreen()); }
        void MajorEntry4Selected(object sender, EventArgs e)
        { screenManager.AddScreen(new Blank4EntryScreen()); }
        void MajorEntry5Selected(object sender, EventArgs e)
        { screenManager.AddScreen(new Blank5EntryScreen()); }
        #endregion

        #region Handle Input
        // Handle input.
        public override void HandleInput(InputState input, GameTime gameTime)
        {
            HandleMajorMenuEntryInput(input);
        }

        protected void HandleMajorMenuEntryInput(InputState input)
        {
            // Move to the previous menu entry?
            if (input.IsKeyUp() || input.IsKeyLeft())
            {
                selectedMajorMenuEntry--;
                screenManager.MenuItemSound.Play(0.5f, 0.0f, 0.0f);

                if (selectedMajorMenuEntry < 0)
                    selectedMajorMenuEntry = menuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (input.IsKeyDown() || input.IsKeyRight())
            {
                selectedMajorMenuEntry++;
                screenManager.MenuItemSound.Play(0.5f, 0.0f, 0.0f);

                if (selectedMajorMenuEntry >= menuEntries.Count)
                    selectedMajorMenuEntry = 0;
            }

            // Handle selecting menu entries to proceed to next screen, or exiting from this screen.
            if (input.IsMenuSelect())
            {
                OnSelectMajorMenuEntry(selectedMajorMenuEntry);
            }
            else if (input.IsMenuCancel())
            {
                OnCancel();
            }
        }
        #endregion

        #region Other Methods
        protected void OnSelectMajorMenuEntry(int entryIndex)
        {
            menuEntries[entryIndex].OnSelectEntry();
        }

        protected virtual void OnCancel()
        {
            ExitScreen();
        }
        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedMajorMenuEntry);

                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedMajorMenuEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            spriteBatch.DrawString(this.ScreenManager.Font, "Exp: " + currentXP + "/" + nextLevelXP, xpPosition, Color.White);
            moniesPosition.X = ScreenManager.GraphicsDevice.Viewport.Width - ScreenManager.Font.MeasureString("Monies "+currentMonies+" ").X;
            spriteBatch.DrawString(this.ScreenManager.Font, "Monies: " + currentMonies, moniesPosition, Color.White);
            spriteBatch.End();
        }

        protected virtual void UpdateMenuEntryLocations()
        {
            // Beginning position of the menu entries.
            Vector2 position = new Vector2(10f, 45f);

            // update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                // set the entry's position
                menuEntry.Position = position;

                position.X += menuEntry.GetWidth(this) + 8;
            }
        }
        #endregion
    }
}
