using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    class BattleScreenEnemy : BattleScreenMember
    {
        int selectedTarget;
        Random gen;

        public BattleScreenEnemy(String textureFile, BattleScreen owner, int strength, int defense, int health)
            : base(textureFile, owner)
        {
            maxHP = health;
            currentHP = maxHP;
            this.Strength = strength;
            this.Defense = defense;
            NameText = "Enemy";
            OwnerScreen = owner;
            IsPlayer = false;
            IsPlayerCharacter = false;
            combatActions = new CombatAction[2];
            Position = new Vector2(400, 200);
            InitializeCombatActions();
            gen = new Random(DateTime.Now.Millisecond);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            drawBox = new Rectangle(0, 0, texture.Width, Texture.Height);
        }

        public override void Update(GameTime gameTime)
        {
            if (HasCurrentTurn)
            {
                // Default attack player.
                selectedTarget = OwnerScreen.BattleMembers.FindIndex(FindPlayer);
                CurrentCombatAction = CombatActions[0];

                if (currentHP != maxHP)
                {
                    bool attack = gen.NextDouble() > 0.2;
                    if (attack)
                    {
                        if (gen.Next(2) == 1) {
                            selectedTarget = OwnerScreen.BattleMembers.FindIndex(FindAIParty);
                        }
                    }
                    else
                    {
                        selectedTarget = OwnerScreen.BattleMembers.FindIndex(FindSelf);
                        CurrentCombatAction = CombatActions[1];
                    }
                }
                CurrentCombatAction.PerformAction(this, OwnerScreen.BattleMembers[selectedTarget], CurrentCombatAction);
                OwnerScreen.AdvanceTurn();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            //spriteBatch.Draw(Texture, Position, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, 0.0f, new Vector2(0, 0), 2.5f, SpriteEffects.None, 0.0f);
        }

        void InitializeCombatActions()
        {
            for (int i = 0; i < CombatActions.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        combatActions[i] = new CombatAction(this, "Attack");
                        break;
                    case 1:
                        combatActions[i] = new CombatAction(this, "Heal");
                        break;
                }
            }
        }

        bool FindPlayer(BattleScreenMember member)
        {
            if (member.IsPlayer)
            { return true; }
            return false;
        }

        bool FindSelf(BattleScreenMember member)
        {
            if (!member.IsPlayer && !member.IsPlayerCharacter)
                return true;
            return false;
        }

        bool FindAIParty(BattleScreenMember member)
        {
            if (!member.IsPlayer && member.IsPlayerCharacter)
                return true;
            return false;
        }
    }
}