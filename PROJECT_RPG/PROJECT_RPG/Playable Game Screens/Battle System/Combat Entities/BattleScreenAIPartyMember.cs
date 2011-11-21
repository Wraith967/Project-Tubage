using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    class BattleScreenAIPartyMember : BattleScreenMember
    {
        List<int> targets;
        int selectedTarget;
        Random gen;

        public BattleScreenAIPartyMember(string textureFileName, BattleScreen ownerScreen)
            : base(textureFileName, ownerScreen)
        {
            maxHP = 50;
            currentHP = 50;
            NameText = "Lappy";
            OwnerScreen = owner;
            IsPlayer = false;
            IsPlayerCharacter = true;
            combatActions = new CombatAction[3];
            Position = new Vector2(300, 200);
            InitializeCombatActions();
            targets = new List<int>();
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
                targets.Clear();
                foreach (BattleScreenMember member in OwnerScreen.BattleMembers)
                {
                    if (member is BattleScreenEnemy)
                        targets.Add(OwnerScreen.BattleMembers.IndexOf(member));
                }

                // Default attack enemy.
                selectedTarget = targets.ElementAt(gen.Next(targets.Count));
                CurrentCombatAction = CombatActions[0];

                if (currentHP != maxHP)
                {
                    bool attack = gen.NextDouble() > 0.2;
                    if (attack)
                    {
                        selectedTarget = targets.ElementAt(gen.Next(targets.Count));
                        CurrentCombatAction = CombatActions[0];
                    }
                    else
                    {
                        if (gen.Next(2) == 0)
                        {
                            selectedTarget = OwnerScreen.BattleMembers.FindIndex(FindSelf);
                            CurrentCombatAction = CombatActions[1];
                        }
                        else
                        {
                            selectedTarget = OwnerScreen.BattleMembers.FindIndex(FindPlayer);
                            CurrentCombatAction = CombatActions[2];
                        }
                    }
                }
                CurrentCombatAction.PerformAction(this, OwnerScreen.BattleMembers[selectedTarget], CurrentCombatAction);
                OwnerScreen.AdvanceTurn();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
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
                        combatActions[i] = new CombatAction(this, "Heal Self");
                        break;
                    case 2:
                        combatActions[i] = new CombatAction(this, "Heal Player");
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
            if (!member.IsPlayer && member.IsPlayerCharacter)
                return true;
            return false;
        }

        bool FindEnemy(BattleScreenMember member)
        {
            if (!member.IsPlayer && !member.IsPlayerCharacter)
                return true;
            return false;
        }
    }
}
