using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }

    public abstract class GameScreen
    {
        #region Properties

        // Gets and sets the current screen state.
        protected ScreenState screenState = ScreenState.TransitionOn;
        public ScreenState ScreenState
        {
            get { return screenState; }
            set { screenState = value; }
        }

        // Gets and sets the screen manager.
        protected ScreenManager screenManager;
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            set { screenManager = value; }
        }

        // How long the screen takes to transition off.
        protected TimeSpan transitionOffTime = TimeSpan.Zero;
        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            set { transitionOffTime = value; }
        }

        // How long the screen takes to transition on.
        protected TimeSpan transitionOnTime = TimeSpan.Zero;
        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            set { transitionOnTime = value; }
        }

        // The current position of the screen transition.
        // Range: 0 (fully on and active) to 1 (fully off and not active).
        protected float transitionPosition = 1;
        public float TransitionPosition
        {
            get { return transitionPosition; }
            set { transitionPosition = value; }
        }

        // Gets the alpha of the screen transition (fading).
        // Range 1 to 0, fully on to fully off.
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }

        // Whether or not the screen is transitioning off to actually be removed.
        protected bool isExiting = false;
        public bool IsExiting
        {
            get { return isExiting; }
            set { isExiting = value; }
        }

        // isPopop defines whether or not the screen is a popup screen: so the original screen does not transition off.
        protected bool isPopup = false;
        public bool IsPopup
        {
            get { return isPopup; }
            set { isPopup = value; }
        }

        // Checks whether screen can respond to user input. Makes use of otherScreenHasFocus to make sure
        // things like a screen below a popup screen isn't active.
        protected bool otherScreenHasFocus;
        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus &&
                    (screenState == ScreenState.TransitionOn || screenState == ScreenState.Active);
            }
        }

        #endregion

        #region Initialization

        // Load graphics, sounds, etc.
        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }
        #endregion

        #region Update and Draw
        
        // 
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (isExiting)
            {
                // If the screen is going away to be removed, it should transition off.
                screenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    // When the transition finishes, remove the screen.
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (coveredByOtherScreen)
            {
                // If the screen is covered by another, it should transition off.
                if (UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    // Still busy transitioning.
                    screenState = ScreenState.TransitionOff;
                }
                else
                {
                    // Transition finished!
                    screenState = ScreenState.Hidden;
                }
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    // Still busy transitioning.
                    screenState = ScreenState.TransitionOn;
                }
                else
                {
                    // Transition finished!
                    screenState = ScreenState.Active;
                }
            }
        }

        
        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
 	        // How much should we move by?
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

            // Update the transition position.
            transitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (transitionPosition <= 0)) ||
                ((direction > 0) && (transitionPosition >= 1)))
            {
                // Can only be between not-finished and finished.
                transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
                return false;
            }
            
            // Otherwise we are still busy transitioning.
            return true;
        }

        // Handles user input; only called when screen is active.
        public virtual void HandleInput(InputState input, GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) { }

        #endregion

        #region Public Methods

        // Screen transitions off then dies.
        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                // If the screen has a zero transition time, remove it immediately.
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                isExiting = true;
            }
        }
        		 
	    #endregion

    }
}
