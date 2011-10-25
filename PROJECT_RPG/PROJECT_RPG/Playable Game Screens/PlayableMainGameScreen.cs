using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

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

        // General map related fields and properties.
        const int mapHeightInPixels = 680;
        const int mapWidthInPixels = 480;
        string mapTextureName;
        MapTile[,] tileMap;
        List<DrawableEntity> entities = new List<DrawableEntity>();
        List<DrawableEntity> entitiesToUpdate = new List<DrawableEntity>();
        PlayerEntity player;
        Vector2 playerPos;




        #endregion

        #region Initialization

        // Constructor
        public PlayableMainGameScreen(string mapTextureName, Vector2 playerPos)
        {
            this.mapTextureName = mapTextureName;
            tileMap = MapReader.readTileMap(mapTextureName, this);
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0);
            this.playerPos = playerPos;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            ContentManager = new ContentManager(ScreenManager.Game.Services, "Content");
            font = ScreenManager.Font;

            AddEntity(new PlayerEntity("cats", playerPos));

            foreach (DrawableEntity entity in entities)
            {
                entity.LoadContent();
            }
            foreach (MapTile m in tileMap)
                m.LoadContent(ContentManager);

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
                // Handle updating of each drawable game entity.
                entitiesToUpdate.Clear();
                foreach (DrawableEntity entity in entities)
                    entitiesToUpdate.Add(entity);
                while (entitiesToUpdate.Count > 0)
                {
                    DrawableEntity entity = entitiesToUpdate[entitiesToUpdate.Count - 1];
                    entitiesToUpdate.RemoveAt(entitiesToUpdate.Count - 1);
                    entity.Update(gameTime);
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
            foreach (MapTile m in tileMap)
                m.Draw(spriteBatch);
            spriteBatch.End();

            // Handle drawing of each drawable game entity.
            foreach (DrawableEntity entity in entities)
            {
                entity.Draw(gameTime);
            }
            // Done.

            // If the game is transitioning on, fade it in.
            if (TransitionPosition > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, 0);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
            if (input.IsPauseButtonPressed())
            {
                screenManager.AddScreen(new PauseScreen());
            }
            if (input.IsInGameMenuButtonPressed())
            { screenManager.AddScreen(new InGameMenuScreen()); }
            player.HandleInput(input);
            CheckForCollision();
        }

        private bool IsCollision(int xCoord, int yCoord)
        {
            return tileMap[yCoord, xCoord].CollisionCheck(player.getBoundary);
        }

        private int CheckForCollision()
        {
            int xCoord = (int)player.Position.X / 20;
            int yCoord = (int)player.Position.Y / 20;

            bool collided = IsCollision(xCoord, yCoord);
            if (collided)
            {
                player.undoMove();
                return 0;
            }
            collided = IsCollision(xCoord + 1, yCoord);
            if (collided)
            {
                player.undoMove();
                return 0;
            }
            collided = IsCollision(xCoord, yCoord + 1);
            if (collided)
            {
                player.undoMove();
                return 0;
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
            MapTile temp = tileMap[xCoord, yCoord];
            temp.IsTransfer = true;
            temp.NextMapFile = nextMap;
            temp.TransferPoint = new Vector2(nextX, nextY);
            tileMap[xCoord, yCoord] = temp;
        }

        #endregion
    }
}
