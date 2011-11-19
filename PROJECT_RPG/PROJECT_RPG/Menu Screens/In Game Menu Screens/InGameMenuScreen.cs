using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PROJECT_RPG
{
    class InGameMenuScreen : GameScreen
    {
        // This whole damn screen is a placeholder.
        // General in game menu stuff.
        // Mostly placeholders!

        RectangleOverlay overlay_background;
        RectangleOverlay top;
        RectangleOverlay bot;

        //

        StatBar energyBar_Bill;
        StatBar expBar_Bill;

        Vector2 energyBarPos_Bill;
        Vector2 expBarPos_Bill;

        Vector2 levelPos_Bill;
        Vector2 facePos_Bill;
        Texture2D bill;
        Vector2 strdefPos_Bill;

        //

        StatBar energyBar_Laptop;
        StatBar expBar_Laptop;

        Vector2 energyBarPos_Laptop;
        Vector2 expBarPos_Laptop;

        Vector2 levelPos_Laptop;
        Vector2 facePos_Laptop;
        Texture2D laptop;
        Vector2 strdefPos_Laptop;

        //

        int selectedItem;
        Vector2 itemPosition;
        Vector2 itemDescriptionPosition;
        Texture2D temp_item;

        //

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

            // Differentiate menu entries in this menu from main menu entries.
            foreach (MenuEntry entry in menuEntries)
            { entry.IsPulseMenuEntry = false; entry.ScaleFactor = 1.25f; }
        }

        public override void LoadContent()
        {
            ContentManager content = new ContentManager(ScreenManager.Game.Services, "Content");
            bill = content.Load<Texture2D>("bill");
            laptop = content.Load<Texture2D>("laptop");
            // Load item icons here.
            temp_item = content.Load<Texture2D>("inv_items/energy_drink_icon");

            //

            overlay_background = new RectangleOverlay(new Rectangle(0, 0, GlobalConstants.ScreenWidth, GlobalConstants.ScreenHeight), this);
            top = new RectangleOverlay(new Rectangle(6, 6, GlobalConstants.ScreenWidth - 12, GlobalConstants.ScreenHeight - 360), this);
            bot = new RectangleOverlay(new Rectangle(6,  GlobalConstants.ScreenHeight - 351, GlobalConstants.ScreenWidth - 12, 345), this);
            overlay_background.LoadContent();
            top.LoadContent();
            bot.LoadContent();

            //

            energyBar_Bill = new StatBar(new Rectangle(120, 12, 140, 20), this, true);
            energyBar_Bill.LoadContent();
            energyBarPos_Bill = new Vector2(120, 33);

            expBar_Bill = new StatBar(new Rectangle(120, 55, 140, 20), this, false);
            expBar_Bill.LoadContent();
            expBarPos_Bill = new Vector2(120, 76);

            facePos_Bill = new Vector2(12, 12);
            levelPos_Bill = new Vector2(facePos_Bill.X, facePos_Bill.Y + 95);
            strdefPos_Bill = new Vector2();

            //

            energyBar_Laptop = new StatBar(new Rectangle(488, 12, 140, 20), this, true);
            energyBar_Laptop.LoadContent();
            energyBarPos_Laptop = new Vector2(488, 33);

            expBar_Laptop = new StatBar(new Rectangle(488, 55, 140, 20), this, false);
            expBar_Laptop.LoadContent();
            expBarPos_Laptop = new Vector2(488, 76);

            facePos_Laptop = new Vector2(380, 12);
            levelPos_Laptop = new Vector2(facePos_Bill.X, facePos_Bill.Y + 95);
            strdefPos_Laptop = new Vector2();

            //

            selectedItem = 0;
            itemPosition = new Vector2(12, GlobalConstants.ScreenHeight - 339);
            itemDescriptionPosition = new Vector2(itemPosition.X + temp_item.Width + 10, itemPosition.Y);

            //
        }

        #region Handle Input Events
        // Handle input events.
        void CharacterStatsSelected(object sender, EventArgs e)
        { screenManager.AddScreen(new CharacterStatsMenuScreen()); }

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
                AudioManager.PlaySound("beep");

                if (selectedMajorMenuEntry < 0)
                    selectedMajorMenuEntry = menuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (input.IsKeyDown() || input.IsKeyRight())
            {
                selectedMajorMenuEntry++;
                AudioManager.PlaySound("beep");

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

            // fix item icon locations
            itemPosition.X = 12;
            itemPosition.Y = GlobalConstants.ScreenHeight - 339;

            // fix item desc locations
            itemDescriptionPosition.X = itemPosition.X + temp_item.Width + 10;
            itemDescriptionPosition.Y = itemPosition.Y;

            //

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            //

            overlay_background.Draw();
            top.Draw();
            bot.Draw();
            energyBar_Bill.Draw();
            expBar_Bill.Draw();
            energyBar_Laptop.Draw();
            expBar_Laptop.Draw();

            spriteBatch.DrawString(font, "" + PlayerStats.CurrentEnergy + "/" + PlayerStats.MaximumEnergy, energyBarPos_Bill, Color.White, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, "" + PlayerStats.CurrentXP + "/" + PlayerStats.NextLevelXP, expBarPos_Bill, Color.White, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, "Lvl " + PlayerStats.Level, levelPos_Bill, Color.White, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
            //spriteBatch.DrawString(font, "" + PlayerStats.CurrentEnergy + "/" + PlayerStats.MaximumEnergy, energyBarPos_Bill, Color.White, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
            //spriteBatch.DrawString(font, "" + PlayerStats.CurrentXP + "/" + PlayerStats.NextLevelXP, expBarPos_Bill, Color.White, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
            //spriteBatch.DrawString(font, "Lvl " + PlayerStats.Level, levelPos_Bill, Color.White, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);

            //



            //

            spriteBatch.Draw(bill, facePos_Bill, Color.White);
            spriteBatch.Draw(laptop, facePos_Laptop, Color.White);

            //

            spriteBatch.Draw(temp_item, itemPosition, Color.White);
            spriteBatch.DrawString(font, "Item", itemDescriptionPosition, Color.White);

            // Draw item icons / text.
            for (int i = 0; i < 4; i++)
            {
                // fix item icon locations
                itemPosition.X = 12;

                // fix item desc locations
                itemDescriptionPosition.X = itemPosition.X + temp_item.Width + 10;

                for (int j = 0; j < 4; j++)
                {
                    spriteBatch.Draw(temp_item, itemPosition, Color.White);
                    spriteBatch.DrawString(font, "Item", itemDescriptionPosition, Color.White);
                    itemPosition.X += 160;
                    itemDescriptionPosition.X = itemPosition.X + temp_item.Width + 10;
                }
                itemPosition.Y += 86;
                itemDescriptionPosition.Y = itemPosition.Y;
            }

            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedMajorMenuEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

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
