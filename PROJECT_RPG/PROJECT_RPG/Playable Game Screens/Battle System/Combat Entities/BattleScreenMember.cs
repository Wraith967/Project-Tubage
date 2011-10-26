using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PROJECT_RPG
{
    class BattleScreenMember : DrawableBattleEntity
    {
        protected int maxHP;
        public int MaxHP
        { get { return maxHP; } }

        protected int currentHP;
        public int CurrentHP
        { get { return currentHP; } }
        public void TakeDamage(int dmg)
        {
            currentHP -= dmg;
            if (currentHP <= 0)
            {
                currentHP = 0;
                IsDead = true;
            }
        }
        public void HealDamage(int heal)
        {
            currentHP += heal;
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
        }

        protected bool isTargeted;
        public bool IsTargeted
        { get { return isTargeted; } set { isTargeted = value; } }

        protected bool hasTurn = false;
        public bool HasCurrentTurn
        { get { return hasTurn; } set { hasTurn = value; } }

        protected bool isPlayerCharacter;
        public bool IsPlayerCharacter
        { get { return isPlayerCharacter; } set { isPlayerCharacter = value; } }

        protected bool isPlayer;
        public bool IsPlayer
        { get { return isPlayer; } set { isPlayer = value; } }

        protected bool isDead;
        public bool IsDead
        { get { return isDead; } set { isDead = value; } }

        protected bool isPerformingAction;
        public bool IsPerformingAction
        { get { return isPerformingAction; } set { isPerformingAction = value; } }

        protected CombatAction combatAction;
        public CombatAction CurrentCombatAction
        { get { return combatAction; } set { combatAction = value; } }

        protected CombatAction[] combatActions;
        public CombatAction[] CombatActions
        { get { return combatActions; } set { combatActions = value; } }

        protected string nameText;
        public string NameText
        { get { return nameText; } set { nameText = value; } }

        public BattleScreenMember(string textureFileName, BattleScreen ownerScreen) : base(textureFileName)
        {
            owner = ownerScreen;
        }
    }
}
