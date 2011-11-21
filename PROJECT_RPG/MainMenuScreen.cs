using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += NewGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        #endregion

        #region Handle Input

        void NewGameMenuEntrySelected(object sender, EventArgs e)
        { throw new NotImplementedException(); }

        void OptionsMenuEntrySelected(object sender, EventArgs e)
        { 
            //screenManager.AddScreen(new OptionsMenuScreen());

            LoadingScreen.Load(screenManager, new OptionsMenuScreen());
        }

        void OnCancel(object sender, EventArgs e)
        { ScreenManager.Game.Exit(); }

        #endregion


    }
}
