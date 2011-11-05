using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace PROJECT_RPG
{
    public enum BattleState
    {
        Starting,
        PlayerTurn,
        EnemyTurn,
        AITurn,
        Victory,
        Defeat
    }
    class BattleScreen : GameScreen
    {

        #region Fields and Properties

        SpriteBatch spriteBatch;

        BattleState currentBattleState;

        int currentTurnIndex;
        int playerIndex;

        protected BattleUI gui;
        public BattleUI GUI
        { get { return gui; } }

        List<BattleScreenMember> battleScreenMembers = new List<BattleScreenMember>();
        public List<BattleScreenMember> BattleMembers
        { get { return battleScreenMembers; } set { battleScreenMembers = value; } }

        BattleScreenPlayer player;
        //BattleScreenEnemy enemy;

        Texture2D backgroundTexture;
        String textureFile;
        public String Texture
        { set { textureFile = value; } }

        float startingStateTimer = 3000;
        float victoryTimer = 3000;
        float defeatTimer = 3000;

        EnemyEntity creator;
        GameScreen previousScreen;
        String battleFile;

        #endregion

        #region Initialization

        public BattleScreen(String filename, EnemyEntity starter, GameScreen previous)
        {
            TransitionOnTime = TimeSpan.Zero;
            TransitionOffTime = TimeSpan.Zero;

            currentBattleState = BattleState.Starting;
            previousScreen = previous;

            creator = starter;
            battleFile = filename;
        }

        public override void LoadContent()
        {
            ContentManager content = new ContentManager(ScreenManager.Game.Services, "Content");
            spriteBatch = ScreenManager.SpriteBatch;
            BattleScreenLoader.LoadBattle(battleFile, this);
            backgroundTexture = content.Load<Texture2D>(textureFile);

            //player = new BattleScreenPlayer("tempface", this);
            //enemy = new BattleScreenEnemy("evil_face", this);
            //battleScreenMembers.Add(player);
            //battleScreenMembers.Add(enemy);

            InitializeTurnOrder();

            playerIndex = battleScreenMembers.FindIndex(FindPlayer);

            // Combat UI setup.
            gui = new BattleUI(this);
            gui.LoadContent();

            foreach (BattleScreenMember d in BattleMembers)
            {
                d.LoadContent();
            }
        }

        public override void UnloadContent()
        {
        }

        public void AddBattleMember(BattleScreenMember member)
        {
            battleScreenMembers.Add(member);
            if (member is BattleScreenPlayer)
                player = (BattleScreenPlayer)member;
        }

        #endregion

        #region Combat State Logic

        void InitializeTurnOrder()
        {
            List<BattleScreenMember> temp = new List<BattleScreenMember>();
            foreach (BattleScreenMember combatant in battleScreenMembers)
            {
                if (combatant is BattleScreenPlayer)
                {
                    temp.Insert(0, combatant);
                }
                else if (combatant is BattleScreenEnemy)
                {
                    temp.Add(combatant);
                }
            }
            BattleMembers = temp;
        }

        public void AdvanceTurn()
        {
            battleScreenMembers[currentTurnIndex].IsPerformingAction = false;
            battleScreenMembers[currentTurnIndex].HasCurrentTurn = false;
            currentTurnIndex++;
            if (currentTurnIndex >= 2)
            { currentTurnIndex = 0; }
            battleScreenMembers[currentTurnIndex].HasCurrentTurn = true;
            if (battleScreenMembers[currentTurnIndex].IsPlayer)
            {
                currentBattleState = BattleState.PlayerTurn;
            }
            else if (battleScreenMembers[currentTurnIndex].IsPlayerCharacter
                && !battleScreenMembers[currentTurnIndex].IsPlayer)
            {
                currentBattleState = BattleState.AITurn;
            }
            else if (!battleScreenMembers[currentTurnIndex].IsPlayerCharacter
                && !battleScreenMembers[currentTurnIndex].IsPlayer)
            {
                currentBattleState = BattleState.EnemyTurn;
            }
        }

        void CheckVictoryConditions()
        {
            if (battleScreenMembers[playerIndex].IsDead)
            {
                currentBattleState = BattleState.Defeat;
            }
            int enemyCount = 0;
            foreach (BattleScreenMember member in battleScreenMembers)
            {
                if (!member.IsPlayer && !member.IsPlayerCharacter)
                {
                    enemyCount++;
                }
            }
            int deadEnemyCount = 0;
            foreach (BattleScreenMember member in battleScreenMembers)
            {
                if (!member.IsPlayer && !member.IsPlayerCharacter)
                {
                    if (member.IsDead)
                    { deadEnemyCount++; }
                }
            }
            if (deadEnemyCount == enemyCount)
            {
                currentBattleState = BattleState.Victory;
            }
        }

        bool FindPlayer(BattleScreenMember member)
        {
            if (member.IsPlayer)
            { return true; }
            return false;
        }

        #endregion

        #region Update and Draw

        public override void HandleInput(InputState input, GameTime gameTime)
        {
            switch (currentBattleState)
            {
                case BattleState.Starting:
                    break;
                case BattleState.PlayerTurn:
                    player.HandleInput(input);
                    break;
                case BattleState.EnemyTurn:
                    break;
                case BattleState.AITurn:
                    break;
                case BattleState.Victory:
                    break;
                case BattleState.Defeat:
                    break;
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            switch (currentBattleState)
            {
                case BattleState.Starting:
                    startingStateTimer -= gameTime.ElapsedGameTime.Milliseconds;
                    if (startingStateTimer <= 0)
                    {
                        BattleMembers[0].HasCurrentTurn = true;
                        currentBattleState = BattleState.PlayerTurn;
                        currentTurnIndex = 0;
                    }
                    break;
                case BattleState.PlayerTurn:
                    CheckVictoryConditions();
                    break;
                case BattleState.EnemyTurn:
                    CheckVictoryConditions();
                    break;
                case BattleState.AITurn:
                    CheckVictoryConditions();
                    break;
                case BattleState.Victory:
                    victoryTimer -= gameTime.ElapsedGameTime.Milliseconds;
                    if (victoryTimer <= 0)
                    {
                        if (previousScreen is PlayableMainGameScreen)
                            ((PlayableMainGameScreen)previousScreen).RemoveEntity(creator);
                        ScreenManager.RemoveScreen(this);
                        //ScreenManager.AddScreen(new MainMenuScreen());
                    }
                    break;
                case BattleState.Defeat:
                    defeatTimer -= gameTime.ElapsedGameTime.Milliseconds;
                    if (defeatTimer <= 0)
                    {
                        ScreenManager.RemoveScreen(this);
                        ScreenManager.AddScreen(new MainMenuScreen());
                    }
                    break;
            }
            foreach (BattleScreenMember m in BattleMembers)
            {
                m.Update(gameTime);
            }
            gui.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            // Draw background of the battle.
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

            gui.Draw(spriteBatch);

            // Draw each player and enemy character.
            foreach (BattleScreenMember character in BattleMembers)
            {
                character.Draw(gameTime, spriteBatch);
            }

            if (currentBattleState == BattleState.Victory)
            {
                spriteBatch.DrawString(ScreenManager.Font, "You win!", Vector2.Zero, Color.DarkBlue);
            }
            else if (currentBattleState == BattleState.Defeat)
            {
                spriteBatch.DrawString(ScreenManager.Font, "You lose!", Vector2.Zero, Color.DarkBlue);
            }
            spriteBatch.End();
        }

        #endregion
    }
}
