using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PROJECT_RPG
{
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields and Properties

        MenuEntry randomOption1;
        MenuEntry randomOption2;
        MenuEntry randomOption3;
        
        // Static for now, as examples. Probably save real settings to a file or something.
        static int randomInt = 0;
        static int randomIntTwo = 100;

        #endregion

        #region Initialization

        public OptionsMenuScreen() : base("Options")
        {
            randomOption1 = new MenuEntry(string.Empty);
            randomOption2 = new MenuEntry(string.Empty);
            randomOption3 = new MenuEntry(string.Empty);

            SetMenuEntryText();

            randomOption1.Selected += RandomOption1Selected;
            randomOption2.Selected += RandomOption2Selected;
            randomOption3.Selected += RandomOption3Selected;

            MenuEntries.Add(randomOption1);
            MenuEntries.Add(randomOption2);
            MenuEntries.Add(randomOption3);

        }

        void SetMenuEntryText()
        {
            randomOption1.Text = "Sup guise." + randomInt;
            randomOption2.Text = "Howset goin?" + randomIntTwo;
            randomOption3.Text = " !\"#$%&'()*+,-./ \n 0123456789= \n { | ^ } \u007F";
        }
        
        #endregion

        #region Handle Input & OnCancel Override

        void RandomOption1Selected(object sender, EventArgs e)
        {
            randomInt++;
            SetMenuEntryText();
        }

        void RandomOption2Selected(object sender, EventArgs e)
        {
            randomIntTwo++;
            SetMenuEntryText();
        }

        void RandomOption3Selected(object sender, EventArgs e)
        {
            SetMenuEntryText();
        }

        protected override void OnCancel()
        {
            LoadingScreen.Load(screenManager, new MainMenuScreen());
        }
        #endregion


    }
}
