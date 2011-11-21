using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PROJECT_RPG
{
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields and Properties

        MenuEntry MusicVolume;
        MenuEntry SoundsVolume;
        MenuEntry randomOption3;
        
        // Static for now, as examples. Probably save real settings to a file or something.

        #endregion

        #region Initialization

        public OptionsMenuScreen() : base("Options")
        {
            MusicVolume = new MenuEntry(string.Empty);
            SoundsVolume = new MenuEntry(string.Empty);
            randomOption3 = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MusicVolume.Selected += MusicVolumeOptionSelected;
            SoundsVolume.Selected += SoundVolumeOptionSelected;
            randomOption3.Selected += RandomOption3Selected;

            MenuEntries.Add(MusicVolume);
            MenuEntries.Add(SoundsVolume);
            MenuEntries.Add(randomOption3);

        }

        void SetMenuEntryText()
        {
            MusicVolume.Text = "Music Volume:" + AudioManager.Instance.MusicVolume.ToString();
            SoundsVolume.Text = "Sounds Volume:" + AudioManager.Instance.SoundVolume.ToString();
            randomOption3.Text = " !\"#$%&'()*+,-./ \n 0123456789= \n { | ^ } \u007F";
        }
        
        #endregion

        #region Handle Input & OnCancel Override

        void MusicVolumeOptionSelected(object sender, EventArgs e)
        {
            if (AudioManager.Instance.MusicVolume == 1)
            {
                AudioManager.Instance.MusicVolume = 0.0f;
            }
            else AudioManager.Instance.MusicVolume += 0.05f;
            SetMenuEntryText();
        }

        void SoundVolumeOptionSelected(object sender, EventArgs e)
        {
            if (AudioManager.Instance.SoundVolume == 1)
            {
                AudioManager.Instance.SoundVolume = 0.0f;
            }
            else if (AudioManager.Instance.SoundVolume + 0.05 > 1)
            { AudioManager.Instance.SoundVolume = 1; }
            else AudioManager.Instance.SoundVolume += 0.05f;
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
