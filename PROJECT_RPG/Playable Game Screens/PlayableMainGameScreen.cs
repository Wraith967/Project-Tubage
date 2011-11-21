using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace PROJECT_RPG
{
    public enum MapTileCollisionType
    {
        NoCollision,
        HalfCollisionTop,
        HalfCollisionBot,
        HalfCollisionLeft,
        HalfCollisionRight,
        HalfCollisionCornerTopLeft,
        HalfCollisionCornerTopRight,
        HalfCollisionCornerBotLeft,
        HalfCollisionCornerBotRight,
        FullCollision
    }

    class PlayableMainGameScreen : GameScreen
    {
        #region Fields and Properties

        // General stuff needed for the screen
        ContentManager content;
        public ContentManager ContentManager
        {
            get { return content; }
            set { content = value; }
        }

        SpriteFont font;

        int boundarySize = 75;  // pixels from the edge of the screen player can get to befor
                                // the screen scrolls

        // General map related fields and properties.
        int squaresAcross = GlobalConstants.ScreenWidth / 20;
        int squaresDown = GlobalConstants.ScreenHeight / 20;
        int maxCameraX, maxCameraY, mapWidth, mapHeight;
        string screenFile;
        MapTile[,] tileMap;
        List<DrawableEntity> entities = new List<DrawableEntity>();
        List<DrawableEntity> entitiesToUpdate = new List<DrawableEntity>();
        List<NonPlayerEntity> entitiesToCheck = new List<NonPlayerEntity>();
        PlayerEntity player;
        Vector2 playerPos;

        public Vector2 PlayerPos
        { get { return playerPos; } }

        public PlayerEntity Player
        { get { return player; } }

        List<string> songs = new List<string>();
        public List<string> Songs
        { get { return songs; } }

        public string currentSong;

        #endregion

        #region Initialization

        // Constructor
        public PlayableMainGameScreen(string screenFile, Vector2 playerPos)
        {
            this.screenFile = screenFile;
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0);
            this.playerPos = playerPos;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            ContentManager = new ContentManager(ScreenManager.Game.Services, "Content");
            font = ScreenManager.Font;
            EngineLoader.LoadScriptFile(screenFile, this);
            mapWidth = tileMap.GetLength(1);
            mapHeight = tileMap.GetLength(0);
            maxCameraX = (mapWidth - squaresAcross) * 20;
            maxCameraY = (mapHeight - squaresDown) * 20;
            if (maxCameraY < 0)
                maxCameraY = 0;
            if (maxCameraX < 0)
                maxCameraX = 0;

            InitialPosition();

            if (currentSong == null)
            {
                currentSong = songs[0];
            }

            if (AudioManager.Instance.CurrentSong != currentSong)
            {
                // Try loading.
                if (!AudioManager.IsSongLoaded(currentSong))
                    AudioManager.LoadSong(currentSong);

                if (AudioManager.Instance.IsSongPaused)
                {
                    AudioManager.ResumeSong();
                }
                else
                {
                    AudioManager.PlaySong(currentSong, true);
                }
            }
            if (AudioManager.Instance.CurrentSong == currentSong &&
                !AudioManager.Instance.IsSongActive)
            {
                AudioManager.PlaySong(currentSong, true);
            }

            //AddEntity(new PlayerEntity("cats", playerPos));

            foreach (DrawableEntity entity in entities)
            {
                entity.LoadContent();
            }
            foreach (MapTile m in tileMap)
                m.LoadContent();

            ScreenManager.Game.ResetElapsedTime();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            foreach (DrawableEntity entity in entities)
            {
                entity.UnloadContent();
            }
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            
            // Only want to update the screen if it is not paused.
            if (IsActive)
            {
                if (AudioManager.Instance.CurrentSong != currentSong)
                {
                    // Try loading.
                    if (!AudioManager.IsSongLoaded(currentSong))
                    {
                        AudioManager.LoadSong(currentSong);
                    }

                    if (AudioManager.Instance.IsSongPaused)
                    {
                        AudioManager.ResumeSong();
                    }
                    else
                    {
                        AudioManager.PlaySong(currentSong, true);
                    }
                }

                // Handle updating of each drawable game entity.
                entitiesToUpdate.Clear();
                foreach (DrawableEntity entity in entities)
                    entitiesToUpdate.Add(entity);
                while (entitiesToUpdate.Count > 0)
                {
                    DrawableEntity entity = entitiesToUpdate[entitiesToUpdate.Count - 1];
                    entitiesToUpdate.RemoveAt(entitiesToUpdate.Count - 1);
                    entity.Update(gameTime);
                    if (entity is NonPlayerEntity)
                        entitiesToCheck.Add((NonPlayerEntity)entity);

                }
                // Done handling updating of drawable game entities.
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Vector2 cornerVector = Vector2.Zero;

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null);
            //spriteBatch.Draw(mapTexture, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            //spriteBatch.DrawString(font, ScreenManager.GetScreens().Length.ToString(), new Vector2(256,256), Color.Red);

            Vector2 firstSquare = new Vector2(Camera.Position.X / 20, Camera.Position.Y / 20);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            for (int y = 0; y < Math.Min(squaresDown + 1, mapHeight - firstY); y++)
            {
                for (int x = 0; x < Math.Min(squaresAcross + 1, mapWidth - firstX); x++)
                {

                    tileMap[y + firstY, x + firstX].Draw(spriteBatch);
                }
            }
            spriteBatch.End();

            spriteBatch.Begin();
            
            // Handle drawing of each drawable game entity.
            foreach (DrawableEntity entity in entities)
            {
                if (entity is NonPlayerEntity)
                    entity.Draw(gameTime, spriteBatch);
            }
            player.Draw(gameTime, spriteBatch);
            // Done.
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(ScreenManager.Font, MediaPlayer.IsRepeating.ToString(), Vector2.One, Color.Green);
            spriteBatch.End();

            // If the game is transitioning on, fade it in.
            if (TransitionPosition > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, 0);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        //TODO: FIX THIS!!!
        private void InitialPosition()
        {
            if (playerPos.X > (GlobalConstants.ScreenWidth - boundarySize))
            {
                Camera.Position.X = playerPos.X - (GlobalConstants.ScreenWidth / 2);
            }
            if (playerPos.Y > (GlobalConstants.ScreenHeight - boundarySize))
            {
                Camera.Position.Y = playerPos.Y - (GlobalConstants.ScreenHeight / 2);
            }
            Camera.Position.X = MathHelper.Clamp(Camera.Position.X, 0, maxCameraX);
            Camera.Position.Y = MathHelper.Clamp(Camera.Position.Y, 0, maxCameraY);
        }

        public override void HandleInput(InputState input, GameTime gameTime)
        {
            base.HandleInput(input, gameTime);
            if (input.IsPauseButtonPressed())
            {
                AudioManager.PauseSong();
                screenManager.AddScreen(new PauseScreen());
            }
            if (input.IsInGameMenuButtonPressed())
            { screenManager.AddScreen(new InGameMenuScreen()); }
            if (input.IsInGameEscapeButtonPressed())
            { LoadingScreen.Load(ScreenManager, new MainMenuScreen()); }
            player.HandleInput(input, gameTime);
            CheckForCollision();
            HandleScrolling();
        }

        //TODO: FIX THIS SHIT
        private void HandleScrolling()
        {
            if ((Player.Position.X - Camera.Position.X) < boundarySize)
            {
                UpdatePosition(3);
            }
            if ((Player.Position.Y - Camera.Position.Y) < boundarySize)
            {
                UpdatePosition(0);
            }
            if (((Camera.Position.X + GlobalConstants.ScreenWidth) - Player.Position.X) < boundarySize)
            {
                UpdatePosition(1);
            }
            if (((Camera.Position.Y + GlobalConstants.ScreenHeight) - Player.Position.Y) < boundarySize)
            {
                UpdatePosition(2);
            }
        }

        private void UpdatePosition(int direction)
        {
            float delta = GlobalConstants.moveSpeed;
            switch (direction)
            {
                case 0:
                    Camera.Position.Y -= delta;
                    break;
                case 1:
                    Camera.Position.X += delta;
                    break;
                case 2:
                    Camera.Position.Y += delta;
                    break;
                case 3:
                    Camera.Position.X -= delta;
                    break;
            }
            Camera.Position.X = MathHelper.Clamp(Camera.Position.X, 0, maxCameraX);
            Camera.Position.Y = MathHelper.Clamp(Camera.Position.Y, 0, maxCameraY);
        }

        private bool IsCollision(int xCoord, int yCoord)
        {
            return tileMap[yCoord, xCoord].CollisionCheck(player.getBoundary);
        }

        private int CheckForCollision()
        {
            int xCoord = (int)player.Position.X / 20;
            int yCoord = (int)player.Position.Y / 20;
            bool collided;

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    collided = IsCollision(xCoord + x, yCoord + y);
                    if (collided)
                    {
                        player.undoMove();
                    }
                }
            }
            foreach (NonPlayerEntity entity in entitiesToCheck)
            {
                collided = entity.HasCollision();
                if (collided)
                {
                    player.undoMove();
                }
            }
            return 1;
        }

        #endregion

        #region Public Methods

        public void AddEntity(DrawableEntity entity)
        {
            entity.OwnerScreen = this;
            entity.LoadContent();
            entities.Add(entity);
            if (entity is PlayerEntity)
                player = (PlayerEntity)entity;
        }

        public void RemoveEntity(DrawableEntity entity)
        {
            entity.UnloadContent();
            entities.Remove(entity);
            entitiesToUpdate.Remove(entity);
        }

        public void setTileMap(MapTile[,] map)
        {
            tileMap = map;
        }

        public void setTransferPoint(String nextMap, int xCoord, int yCoord, int nextX, int nextY)
        {
            // Because XNA creates arrays row-first, we have to reverse the coords
            MapTile temp = tileMap[yCoord, xCoord];
            temp.IsTransfer = true;
            temp.NextMapFile = nextMap;
            temp.TransferPoint = new Vector2(nextX, nextY);
        }

        public void PlayerInteraction(GameTime gameTime)
        {
            foreach (DrawableEntity entity in entities)
            {
                if (entity is NonPlayerEntity)
                {
                    if ((int)Vector2.Subtract(player.Position, entity.Position).Length() < 50)
                    {
                        ((NonPlayerEntity)entity).Interact(gameTime);
                    }
                }
            }
        }

        #endregion
    }
}
