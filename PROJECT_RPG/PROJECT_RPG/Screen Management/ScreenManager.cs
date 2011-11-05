using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;


namespace PROJECT_RPG
{
    public class ScreenManager : DrawableGameComponent
    {

        #region Fields and Properties

        Texture2D blankTexture;
        SpriteFont font;
        SpriteBatch spriteBatch;
        SoundEffect menuItemSound;
        MapTextures mapTex;

        public MapTextures MapTex
        { get { return mapTex; } }

        InputState input = new InputState();

        bool isInitialized;
        bool screenTraceEnabled;

        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> screensToUpdate = new List<GameScreen>();

        // All of the properties are shared throughout all screens.
        public SpriteFont Font { get { return font; } }

        public SoundEffect MenuItemSound { get { return menuItemSound; } }

        public SpriteBatch SpriteBatch { get { return spriteBatch; } }

        public bool ScreenTraceEnabled { get { return screenTraceEnabled; } set { screenTraceEnabled = value; } }

        #endregion

        #region Initialization

        public ScreenManager(Game game) : base(game)
        { }

        public override void Initialize()
        {
            base.Initialize();
            isInitialized = true;
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mapTex = new MapTextures(content);

            // To do: Load content shared by all screens (texture, font, ...)
            font = content.Load<SpriteFont>("ct_font_alpha3");
            menuItemSound = content.Load<SoundEffect>("ffvii-messagesent");

            
            blankTexture = content.Load<Texture2D>("blank");

            // Tell each of the screens to load their content.
            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
            }
        }

        protected override void UnloadContent()
        {
            // Tell each of the screens to unload their content.
            foreach (GameScreen screen in screens)
            {
                screen.UnloadContent();
            }
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            // Read the keyboard.
            input.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            screensToUpdate.Clear();

            foreach (GameScreen screen in screens)
                screensToUpdate.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (screensToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // let it handle input.
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(input, gameTime);

                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

            // Used for VS10 console output debugging.
            if (screenTraceEnabled)
                TraceScreens();
        }

        // Used for debugging screens, seeing when one transitions on, off, etc.
        void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (GameScreen screen in screens)
                screenNames.Add(screen.GetType().Name);

            Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        #endregion

        #region Public Methods

        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;

            // If we have a graphics device, tell the screen to load content.
            if (isInitialized)
            {
                screen.LoadContent();
            }

            screens.Add(screen);
        }

        public void RemoveScreen(GameScreen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if (isInitialized)
            {
                screen.UnloadContent();
            }

            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }

        // Grabs an array of all the screens. This returns a -copy- of the list in array form.
        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }

        // Helps draw a black screen for fading screens in and out, darkening for pause screens, etc.
        public void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;

            spriteBatch.Begin();

            spriteBatch.Draw(blankTexture,
                             new Rectangle(0, 0, viewport.Width, viewport.Height),
                             Color.Black * alpha);

            spriteBatch.End();
        }

        #endregion
    }
}
