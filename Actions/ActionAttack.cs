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
        int actionTime = 1000;
        int elapsedTime;
        Actor source;
        Actor target;
        int beginHP;
        int targetHP;
        int damage = 20;
        
        public void Initialize(Actor source, Actor target)
        {
            elapsedTime = 0;
            this.source = source;
            this.target = target;
            beginHP = target.health.actorCurrentHealth;
            targetHP = (target.health.actorCurrentHealth - damage <= 0) ? 0 : (target.health.actorCurrentHealth - damage);
        }

        public IActionResult Update()
        {
            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            target.health.actorCurrentHealth = (elapsedTime >= actionTime) ? targetHP : (int)MathHelper.SmoothStep(beginHP, targetHP, (elapsedTime / (float)actionTime));

            if (target.health.actorCurrentHealth == targetHP)
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
