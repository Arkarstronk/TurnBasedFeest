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

        public IActionResult Execute(Actor source, Actor target)
        {
            int damage = 20;

            target.health.actorCurrentHealth -= damage;
            return new ActionResultAttack(damage);
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
            return "Attack";
        }
    }

    class ActionResultAttack : IActionResult
    {
        private int damageDone;

        public ActionResultAttack(int damageDone)
        {
            this.damageDone = damageDone;
        }
    }
}
