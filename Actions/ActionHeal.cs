using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;

namespace TurnBasedFeest.Actions
{
    class ActionHeal : IAction
    {
        int actionTime = 1000;
        int elapsedTime;
        Actor source;
        Actor target;
        int beginHP;
        int targetHP;
        int heal = 20;

        public void Initialize(Actor source, Actor target)
        {
            elapsedTime = 0;
            this.source = source;
            this.target = target;
            beginHP = source.health.actorCurrentHealth;
            targetHP = (source.health.actorCurrentHealth + heal >= source.health.actorMaxHealth) ? source.health.actorMaxHealth : (source.health.actorCurrentHealth + heal);
        }

        public IActionResult Update()
        {
            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            source.health.actorCurrentHealth = (elapsedTime >= actionTime) ? targetHP : (int) MathHelper.SmoothStep(beginHP, targetHP, (elapsedTime / (float)actionTime));

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
