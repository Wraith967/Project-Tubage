using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PROJECT_RPG
{
    class MainMenuScreen : MenuScreen
    {
        #region Initialization

        public MainMenuScreen() : base("Main Menu")
        {
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("New Game");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry testBattleScreenEntry = new MenuEntry("Battle Test");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += NewGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            testBattleScreenEntry.Selected += TestBattleScreenEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(testBattleScreenEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        #endregion

        #region Handle Input

        void NewGameMenuEntrySelected(object sender, EventArgs e)
        { LoadingScreen.Load(screenManager, new PlayableMainGameScreen("testScreen1.txt", new Vector2(256,256))); }

        void OptionsMenuEntrySelected(object sender, EventArgs e)
        { LoadingScreen.Load(screenManager, new OptionsMenuScreen()); }

        void TestBattleScreenEntrySelected(object sender, EventArgs e)
        { ScreenManager.AddScreen(new BattleScreen("testBattle.txt", null, this)); }

        void OnCancel(object sender, EventArgs e)
        { ScreenManager.Game.Exit(); }

        #endregion


    }
}
