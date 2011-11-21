using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Timers;

namespace PROJECT_RPG
{
    class CombatAction
    {
        protected BattleScreenMember user;
        public BattleScreenMember UserCharacter
        { get { return user; } }

        protected BattleScreenMember target;
        public BattleScreenMember TargetCharacter
        { get { return target; } set { target = value; } }

        protected string actionText;
        public string ActionText
        { get { return actionText; } set { actionText = value; } }

        protected bool isHighlighted;
        public bool IsHighLighted
        { get { return isHighlighted; } set { isHighlighted = value; } }

        public event EventHandler<EventArgs> Selected;

        protected internal virtual void OnSelectAction()
        {
            if (Selected != null)
            { Selected(this, new EventArgs()); }
        }

        public CombatAction(BattleScreenMember userOfAction, string action)
        {
            user = userOfAction;
            actionText = action;
        }

        public void PerformAction(BattleScreenMember user, BattleScreenMember target, CombatAction action)
        {
            switch (action.ActionText)
            {
                case "Attack":
                    target.TakeDamage(user.Strength);
                    target.IsTargeted = false;
                    break;
                case "Heal":
                    target.HealDamage(5);
                    target.IsTargeted = false;
                    break;
            }
        }

    }
}
