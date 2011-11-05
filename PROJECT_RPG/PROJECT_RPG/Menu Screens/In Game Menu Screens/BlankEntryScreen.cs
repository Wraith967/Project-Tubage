using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PROJECT_RPG
{
    class BlankEntryScreen : GameScreen
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

        Texture2D playerFace;
        Vector2 playerFaceLocation = new Vector2(10, 85);
        #endregion

        #region Initialization

        public BlankEntryScreen()
        {
            TransitionOffTime = TimeSpan.Zero;
            TransitionOnTime = TimeSpan.Zero;
            IsPopup = true;

            // Menu entry stuff.
            MenuEntry testEntry = new MenuEntry("Stat stuff + leveling stuff here later on, probably. :] ^");
            MenuEntry testEntry2 = new MenuEntry("test");
            MenuEntries.Add(testEntry);
            MenuEntries.Add(testEntry2);

            foreach (MenuEntry entry in MenuEntries)
            { entry.IsPulseMenuEntry = false; entry.ScaleFactor = 1.15f; }
        }

        public override void LoadContent()
        {
            base.LoadContent();
            ContentManager content = new ContentManager(ScreenManager.Game.Services, "Content");
            playerFace = content.Load<Texture2D>("tempface");
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

                if (selectedMajorMenuEntry < 0)
                    selectedMajorMenuEntry = MenuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (input.IsKeyDown() || input.IsKeyLeft())
            {
                selectedMajorMenuEntry++;
                screenManager.MenuItemSound.Play(0.5f, 0.0f, 0.0f);

                if (selectedMajorMenuEntry >= MenuEntries.Count)
                    selectedMajorMenuEntry = 0;
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
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];

                bool isSelected = IsActive && (i == SelectedMajorMenuEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            spriteBatch.Draw(playerFace, playerFaceLocation, Color.White);

            Vector2 tempTextLocation = new Vector2(playerFaceLocation.X+playerFace.Width+5, playerFaceLocation.Y);

            spriteBatch.DrawString(font,
                "NAME\nLEVEL 10\nCURRENTHP / MAXHP\nCURRENTMANAENERGYSKILLPOINTSTUFF / MAXOFTHAT\nETC",
                tempTextLocation, Color.White);

            spriteBatch.End();
            
        }

        protected void UpdateMenuEntryLocations()
        {
            // Beginning position of the menu entries.
            Vector2 position = new Vector2(10f, 185f);

            // update each menu entry's location in turn
            for (int i = 0; i < MenuEntries.Count; i++)
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
