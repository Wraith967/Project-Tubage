using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PROJECT_RPG
{
    class Blank2EntryScreen : InGameMenuScreen
    {
        bool isActiveEntryScreen;
        public bool IsActiveEntryScreen
        { get { return isActiveEntryScreen; } set { isActiveEntryScreen = value; } }

        public Blank2EntryScreen()
        {
            TransitionOffTime = TimeSpan.Zero;
            TransitionOnTime = TimeSpan.Zero;
            IsPopup = true;
            MenuEntry testEntry = new MenuEntry("test");
            MenuEntry testEntry2 = new MenuEntry("test");

            MenuEntries.Add(testEntry);
            MenuEntries.Add(testEntry2);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 position = new Vector2(10, 85);
        }
    }
}
