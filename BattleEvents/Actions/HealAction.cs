using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.GameEvents.Battle;

namespace TurnBasedFeest.BattleEvents.Actions
{
    class HealAction : IAction
    {
        int eventTime = 1000;
        int elapsedTime;
        Actor source;
        Actor target;
        int beginHP;
        int targetHP;
        int heal = 0;

        public void SetActors(Actor source, Actor target)
        {
            this.source = source;
            this.target = target;
        }

        public void Initialize()
        {
            elapsedTime = 0;
            heal = source.GetStats()[StatisticAttribute.SUPPORT_MAGIC];
            beginHP = (int)target.health.CurrentHealth;
            targetHP = (int)((target.health.CurrentHealth + heal >= target.health.MaxHealth) ? target.health.MaxHealth : (target.health.CurrentHealth + heal));            
            target.health.SetColor(Color.Green);
        }

        public bool Update(BattleTurnEvent battle, Input input)
        {
            battle.PushTextUpdate($"{source.name} used {GetName()}");

            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            target.health.CurrentHealth = MathHelper.SmoothStep(beginHP, targetHP, (elapsedTime / (float)eventTime));

            if (elapsedTime >= eventTime)
            {
                target.health.CurrentHealth = targetHP;
                target.health.SetColor(Color.White);

                battle.CurrentActor.battleEvents.RemoveAt(battle.eventIndex);
                battle.eventIndex--;
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool IsSupportive()
        {
            return true;
        }

        public string GetName()
        {
            return "Heal";
        }

        public void Draw(BattleTurnEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
            spritebatch.DrawString(font, heal.ToString(), target.position + new Vector2(0, -(elapsedTime / (float)eventTime) * 50 + 20), Color.White, 0, new Vector2(), 2, SpriteEffects.None, 1);
        }
    }
}
