using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    class BattleScreenPlayer : BattleScreenMember
    {
        SpriteFont font;
        int selectedAction;

        RectangleOverlay actionMenu;
        Vector2 actionMenuPos;

        bool selectingTarget;
        int selectedTarget;

        public BattleScreenPlayer(String textureFile, BattleScreen owner)
            : base(textureFile, owner)
        {
            maxHP = 100;
            currentHP = 100;
            //TextureFileName = "tempface";
            NameText = "Bill";
            OwnerScreen = owner;
            IsPlayer = true;
            IsPlayerCharacter = true;
            combatActions = new CombatAction[4];
            Position = new Vector2(200, 200);
            InitializeCombatActions();
            font = OwnerScreen.ScreenManager.Font;

            actionMenu = new RectangleOverlay(new Rectangle(
               350,
               OwnerScreen.ScreenManager.GraphicsDevice.Viewport.Height - 100,
               115,
               100),
               OwnerScreen);

            combatActions[0].Selected += CombatActionAttackSelected;
            combatActions[1].Selected += CombatActionHealSelected;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            actionMenu.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < CombatActions.Count(); i++)
            {
                CombatActions[i].IsHighLighted = (i == selectedAction);
            }
            if (selectingTarget)
            {
                for (int i = 0; i < OwnerScreen.BattleMembers.Count; i++)
                {
                    OwnerScreen.BattleMembers[i].IsTargeted = (i == selectedTarget);
                }
            }
        }

        public void HandleInput(InputState input)
        {
            if (selectingTarget)
            {
                HandleTargetSelection(input);
            }
            else
            {
                HandleActionMenuInput(input);
            }
        }

        protected void HandleActionMenuInput(InputState input)
        {
            // Move to the previous menu entry?
            if (input.IsKeyUp())
            {
                selectedAction--;
                //screenManager.MenuItemSound.Play(0.5f, 0.0f, 0.0f);

                if (selectedAction < 0)
                    selectedAction = CombatActions.Count() - 1;
            }

            // Move to the next menu entry?
            if (input.IsKeyDown())
            {
                selectedAction++;
                //screenManager.MenuItemSound.Play(0.5f, 0.0f, 0.0f);

                if (selectedAction >= CombatActions.Count())
                    selectedAction = 0;
            }

            // Handle selecting menu entries to proceed to next screen, or exiting from this screen.
            if (input.IsMenuSelect())
            {
                OnSelectAction(selectedAction);
            }

            //else if (input.IsMenuCancel())
            //{
            //    OnCancel();
            //} 
        }

        protected void HandleTargetSelection(InputState input)
        {
            if (input.IsKeyUp())
            {
                selectedTarget--;
                if (selectedTarget < 0)
                    selectedTarget = OwnerScreen.BattleMembers.Count - 1;
            }
            if (input.IsKeyDown())
            {
                selectedTarget++;
                if (selectedTarget >= OwnerScreen.BattleMembers.Count())
                    selectedTarget = 0;
            }
            if (input.IsMenuSelect())
            {
                // Logic to do with performing an action...
                CurrentCombatAction.PerformAction(this, OwnerScreen.BattleMembers[selectedTarget], CurrentCombatAction);
                // Reset shit so it doesn't stick on any menu.
                selectingTarget = false;
                // Go go go.
                OwnerScreen.AdvanceTurn();
            }
        }

        protected void OnSelectAction(int action)
        {
            CombatActions[action].OnSelectAction();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(Texture, Position, Color.White);
            if (HasCurrentTurn)
            {
                DrawActionMenu(this, spriteBatch);
            }

        }

        public void DrawActionMenu(BattleScreenPlayer player, SpriteBatch spriteBatch)
        {
            actionMenuPos = new Vector2(actionMenu.GetInnerRectangle.X + 5, actionMenu.GetInnerRectangle.Y + 5);
            actionMenu.Draw();
            foreach (CombatAction action in player.CombatActions)
            {
                if (action.IsHighLighted)
                {
                    spriteBatch.DrawString(font, action.ActionText, actionMenuPos, Color.Yellow, 0.0f, Vector2.Zero, 1.8f, SpriteEffects.None, 0.0f);
                }
                else
                {
                    spriteBatch.DrawString(font, action.ActionText, actionMenuPos, Color.White, 0.0f, Vector2.Zero, 1.8f, SpriteEffects.None, 0.0f);
                }
                actionMenuPos.Y += font.LineSpacing * 2.0f;
            }
        }

        void InitializeCombatActions()
        {
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        combatActions[i] = new CombatAction(this, "Attack");
                        combatActions[i].IsHighLighted = true;
                        break;
                    case 1:
                        combatActions[i] = new CombatAction(this, "Heal");
                        break;
                    case 2:
                        combatActions[i] = new CombatAction(this, "Do Nothing");
                        break;
                    case 3:
                        combatActions[i] = new CombatAction(this, "Do Nothing");
                        break;
                }
            }
        }

        void CombatActionAttackSelected(object o, EventArgs e)
        {
            selectingTarget = true;
            CurrentCombatAction = (CombatAction)o;
        }

        void CombatActionHealSelected(object o, EventArgs e)
        {
            selectingTarget = true;
            CurrentCombatAction = (CombatAction)o;
        }
    }
}
