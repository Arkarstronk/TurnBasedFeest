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
        public IActionResult Execute(Actor source, Actor target)
        {
            int damage = 20;
            target.health.actorCurrentHealth += damage;
            if (target.health.actorCurrentHealth > target.health.actorMaxHealth)
                target.health.actorCurrentHealth = target.health.actorMaxHealth;
            return new ActionResultHeal(damage);
        }        
        public void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            return "Heal";
        }
    }

    class ActionResultHeal : IActionResult
    {
        private int healed;

        public ActionResultHeal(int healed)
        {
            this.healed = healed;
        }
    }
}
