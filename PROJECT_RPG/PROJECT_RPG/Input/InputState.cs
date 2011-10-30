using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace PROJECT_RPG
{
    public class InputState
    {

        #region Fields

        // We'll use both current and last current keyboard states as overhead to create
        // specific input functionality easier than we would without it; infact some
        // cannot be done without it.
        public KeyboardState CurrentKeyboardState;
        public KeyboardState LastKeyboardState;
        
        #endregion

        #region Initialization

        public InputState()
        {
            CurrentKeyboardState = new KeyboardState();
            LastKeyboardState = new KeyboardState();
        }

        #endregion

        #region Public Methods

        // Reads and sets the latest keyboard state. Update called from the ScreenManager.
        public void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }

        // Query whether or not a certain key has been pressed this update of the keyboard state,
        // that was not pressed the last update keyboard state.
        public bool IsNewKeyPress(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));
        }

        public bool IsKeyHold(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyDown(key));
        }

        public bool IsMenuSelect()
        {
            return (IsNewKeyPress(Keys.Enter) || IsNewKeyPress(Keys.Space));
        }

        public bool IsMenuCancel()
        {
            return (IsNewKeyPress(Keys.Escape));
        }

        public bool IsKeyUp()
        {
            return (IsNewKeyPress(Keys.W) || IsNewKeyPress(Keys.Up));
        }

        public bool IsKeyDown()
        {
            return (IsNewKeyPress(Keys.S) || IsNewKeyPress(Keys.Down));
        }

        public bool IsKeyLeft()
        {
            return (IsNewKeyPress(Keys.A) || IsNewKeyPress(Keys.Left));
        }

        public bool IsKeyRight()
        {
            return (IsNewKeyPress(Keys.D) || IsNewKeyPress(Keys.Right));
        }

        public bool IsHoldUp()
        {
            return (IsKeyHold(Keys.W) || IsKeyHold(Keys.Up));
        }

        public bool IsHoldDown()
        {
            return (IsKeyHold(Keys.S) || IsKeyHold(Keys.Down));
        }

        public bool IsHoldLeft()
        {
            return (IsKeyHold(Keys.A) || IsKeyHold(Keys.Left));
        }

        public bool IsHoldRight()
        {
            return (IsKeyHold(Keys.D) || IsKeyHold(Keys.Right));
        }

        public bool IsPauseButtonPressed()
        {
            return (IsNewKeyPress(Keys.Escape));
        }

        public bool IsUseButtonPressed()
        {
            return (IsNewKeyPress(Keys.E));
        }

        public bool IsInGameMenuButtonPressed()
        {
            return (IsNewKeyPress(Keys.M));
        }

        public bool IsInGameEscapeButtonPressed()
        {
            return (IsNewKeyPress(Keys.Q));
        }


        #endregion

    }
}
