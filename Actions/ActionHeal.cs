using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;

namespace TurnBasedFeest.Actions
{
    class ActionHeal : IAction
    {
        Actor source;
        Actor target;
        int targetHP;
        int heal;

        public void Initialize(Actor source, Actor target)
        {
            this.source = source;
            this.target = target;
            heal = 20;
            targetHP = (source.health.actorCurrentHealth + heal > source.health.actorMaxHealth) ? source.health.actorMaxHealth : (source.health.actorCurrentHealth + heal);
        }

        public IActionResult Execute()
        {
            source.health.actorCurrentHealth += (int)((targetHP - target.health.actorCurrentHealth) * 0.1f);

            if (source.health.actorCurrentHealth == targetHP)
            {
                return new ActionResultHeal(true, heal);
            }
            else
            {
                return new ActionResultHeal(false, heal);
            }

        }
        public string GetName()
        {
            return "Heal";
        }
    }

    class ActionResultHeal : IActionResult
    {
        private int healed;
        private bool isDone;

        public ActionResultHeal(bool done, int healed)
        {
            this.healed = healed;
            this.isDone = done;
        }

        public bool IsDone()
        {
            return isDone;
        }
    }
}
