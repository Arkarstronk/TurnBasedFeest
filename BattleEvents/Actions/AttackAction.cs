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
        Actor source;
        Actor target;
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
            beginHP = (int) target.health.CurrentHealth;
            targetHP = (int) ((target.health.CurrentHealth - damage <= 0) ? 0 : (target.health.CurrentHealth - damage));
            target.health.color = Color.Yellow;
        }

        public bool Update(BattleEvent battle, Input input)
        {
            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            target.health.CurrentHealth = MathHelper.SmoothStep(beginHP, targetHP, (elapsedTime / (float)eventTime));

            if (elapsedTime >= eventTime)
            {
                target.health.color = Color.White;
                target.health.CurrentHealth = targetHP;

                //if this attack killed its target
                if (target.health.CurrentHealth == 0)
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
