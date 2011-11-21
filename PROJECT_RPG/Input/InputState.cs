using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PROJECT_RPG
{
    public class InputState
    {

        #region Fields

        // We'll use both current and last current keyboard states as overhead to create
        // specific input functionality easier than we would without it; infact some
        // cannot be done without it.
        private KeyboardState CurrentKeyboardState;
        private KeyboardState LastKeyboardState;
        private GamePadState CurrentGamePadState;
        private GamePadState LastGamePadState;
        
        #endregion

        #region Initialization

        public InputState()
        {
            CurrentKeyboardState = new KeyboardState();
            LastKeyboardState = new KeyboardState();
            CurrentGamePadState = new GamePadState();
            LastGamePadState = new GamePadState();
        }

        #endregion

        #region Public Methods

        // Reads and sets the latest keyboard state. Update called from the ScreenManager.
        public void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
            LastGamePadState = CurrentGamePadState;
            CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
        }

        // Query whether or not a certain key has been pressed this update of the keyboard state,
        // that was not pressed the last update keyboard state.
        private bool IsNewKeyPress(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));
        }

        private bool IsNewButtonPress(Buttons button)
        {
            return (CurrentGamePadState.IsButtonDown(button) && LastGamePadState.IsButtonUp(button));
        }

        private bool IsKeyHold(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyDown(key));
        }

        private bool IsButtonHold(Buttons button)
        {
            return (CurrentGamePadState.IsButtonDown(button) && LastGamePadState.IsButtonDown(button));
        }

        public bool IsMenuSelect()
        {
            return (IsNewKeyPress(Keys.Enter) || IsNewKeyPress(Keys.Space) || IsNewButtonPress(Buttons.A));
        }

        public bool IsMenuCancel()
        {
            return (IsNewKeyPress(Keys.Escape) || IsNewButtonPress(Buttons.Back));
        }

        public bool IsKeyUp()
        {
            return (IsNewKeyPress(Keys.W) || IsNewKeyPress(Keys.Up) || IsNewButtonPress(Buttons.DPadUp) || IsNewButtonPress(Buttons.LeftThumbstickUp));
        }

        public bool IsKeyDown()
        {
            return (IsNewKeyPress(Keys.S) || IsNewKeyPress(Keys.Down) || IsNewButtonPress(Buttons.DPadDown) || IsNewButtonPress(Buttons.LeftThumbstickDown));
        }

        public bool IsKeyLeft()
        {
            return (IsNewKeyPress(Keys.A) || IsNewKeyPress(Keys.Left) || IsNewButtonPress(Buttons.LeftThumbstickLeft) || IsNewButtonPress(Buttons.DPadLeft));
        }

        public bool IsKeyRight()
        {
            return (IsNewKeyPress(Keys.D) || IsNewKeyPress(Keys.Right) || IsNewButtonPress(Buttons.DPadRight) || IsNewButtonPress(Buttons.LeftThumbstickRight));
        }

        public bool IsHoldUp()
        {
            return (IsKeyHold(Keys.W) || IsKeyHold(Keys.Up) || IsButtonHold(Buttons.DPadUp) || IsButtonHold(Buttons.LeftThumbstickUp));
        }

        public bool IsHoldDown()
        {
            return (IsKeyHold(Keys.S) || IsKeyHold(Keys.Down) || IsButtonHold(Buttons.DPadDown) || IsButtonHold(Buttons.LeftThumbstickDown));
        }

        public bool IsHoldLeft()
        {
            return (IsKeyHold(Keys.A) || IsKeyHold(Keys.Left) || IsButtonHold(Buttons.DPadLeft) || IsButtonHold(Buttons.LeftThumbstickLeft));
        }

        public bool IsHoldRight()
        {
            return (IsKeyHold(Keys.D) || IsKeyHold(Keys.Right) || IsButtonHold(Buttons.DPadRight) || IsButtonHold(Buttons.LeftThumbstickRight));
        }

        public bool IsPauseButtonPressed()
        {
            return (IsNewKeyPress(Keys.Escape) || IsNewButtonPress(Buttons.Start));
        }

        public bool IsUseButtonPressed()
        {
            return (IsNewKeyPress(Keys.E) || IsNewButtonPress(Buttons.A));
        }

        public bool IsInGameMenuButtonPressed()
        {
            return (IsNewKeyPress(Keys.M) || IsNewButtonPress(Buttons.Y));
        }

        public bool IsInGameEscapeButtonPressed()
        {
            return (IsNewKeyPress(Keys.Q) || IsNewButtonPress(Buttons.X));
        }


        #endregion

    }
}
