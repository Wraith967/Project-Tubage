using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PROJECT_RPG
{
    public static class GlobalConstants
    {
        public const int ScreenWidth = 640;
        public const int ScreenHeight = 480;
        public const float moveSpeed = 2.0f;
        public const int tileSize = 20;
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GlobalConstants.ScreenWidth;
            graphics.PreferredBackBufferHeight = GlobalConstants.ScreenHeight;

            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);
            AudioManager.Initialize(this);
            //screenManager.ScreenTraceEnabled = true;
            screenManager.ScreenTraceEnabled = false;

            // Activate the first screens.
            //screenManager.AddScreen(new TitleScreen());
            screenManager.AddScreen(new MainMenuScreen());
            //screenManager.AddScreen(new PlayableMainGameScreen("neighborhoodScreen.txt", new Vector2(3, 20)));
        }


        protected override void Initialize()
        {
 
            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            // TODO: use this.Content to load your game content here
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
