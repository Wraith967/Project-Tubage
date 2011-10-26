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

        public BattleScreenEnemy(BattleScreen owner)
            : base("evil_face", owner)
        {
            maxHP = 50;
            currentHP = 50;
            //TextureFileName = "temp_drawableentity";
            NameText = "Enemy";
            OwnerScreen = owner;
            IsPlayer = false;
            IsPlayerCharacter = false;
            combatActions = new CombatAction[2];
            Position = new Vector2(400, 200);
            InitializeCombatActions();
        }

        public override void Update(GameTime gameTime)
        {
            if (HasCurrentTurn)
            {
                selectedTarget = OwnerScreen.BattleMembers.FindIndex(FindPlayer);
                CurrentCombatAction = CombatActions[0];
                CurrentCombatAction.PerformAction(this, OwnerScreen.BattleMembers[selectedTarget], CurrentCombatAction);
                OwnerScreen.AdvanceTurn();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch spriteBatch = OwnerScreen.ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Position, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, 0.0f, new Vector2(0, 0), 2.5f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }

        void InitializeCombatActions()
        {
            for (int i = 0; i < 2; i++)
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
    }
}
