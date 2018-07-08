using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;


namespace TurnBasedFeest.Actions
{
    class ActionAttack : IAction
    {
        Actor source;
        Actor target;
        int targetHP;
        int damage;
        
        public void Initialize(Actor source, Actor target)
        {
            this.source = source;
            this.target = target;
            damage = 20;
            targetHP = (target.health.actorCurrentHealth - damage < 0) ? 0 : (target.health.actorCurrentHealth - damage);
        }

        public IActionResult Execute()
        {
            target.health.actorCurrentHealth -= (int) ((target.health.actorCurrentHealth - targetHP) * 0.1f);

            if(target.health.actorCurrentHealth == targetHP)
            {
                return new ActionResultAttack(true, damage);
            }
            else
            {
                return new ActionResultAttack(false, damage);
            }
            
        }
        public string GetName()
        {
            return "Attack";
        }
    }

    class ActionResultAttack : IActionResult
    {
        public bool done;
        public int damageDone;

        public ActionResultAttack(bool done, int damageDone)
        {
            this.done = done;
            this.damageDone = damageDone;
        }

        public bool IsDone()
        {
            return done;
        }
    }
}
