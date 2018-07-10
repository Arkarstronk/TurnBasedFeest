using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.GameEvents;

namespace TurnBasedFeest.BattleEvents.Actions
{
    class AttackAction : IAction
    {
        int eventTime = 1000;
        int elapsedTime;
        Actor target;
        Actor source;
        int beginHP;
        int targetHP;
        int damage = 20;

        public void SetActors(Actor source, Actor target)
        {
            this.source = source;
            this.target = target;
        }

        public void Initialize()
        {
            elapsedTime = 0;
            beginHP = (int) target.health.actorCurrentHealth;
            targetHP = (int) ((target.health.actorCurrentHealth - damage <= 0) ? 0 : (target.health.actorCurrentHealth - damage));
        }

        public bool Update(BattleEvent battle, Input input)
        {
            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            target.health.actorCurrentHealth = (elapsedTime >= eventTime) ? targetHP : MathHelper.SmoothStep(beginHP, targetHP, (elapsedTime / (float)eventTime));

            if (elapsedTime >= eventTime)
            {
                //if this attack killed its target
                if(target.health.actorCurrentHealth == 0)
                {
                    battle.currentActor.battleEvents.Insert(battle.eventIndex + 1, new DeathEvent(target));
                }                

                battle.currentActor.battleEvents.RemoveAt(battle.eventIndex);
                battle.eventIndex--;
                return true;
            }
            else
            {
                return false;
            }            
        }

        public string GetName()
        {
            return "Attack";
        }

        public void Draw(BattleEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }

        public bool isPositive()
        {
            return false;
        }
    }
}
