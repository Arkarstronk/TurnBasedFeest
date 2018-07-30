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
            beginHP = (int)target.Health.CurrentHealth;
            targetHP = (int)((target.Health.CurrentHealth + heal >= target.Health.MaxHealth) ? target.Health.MaxHealth : (target.Health.CurrentHealth + heal));            
            target.Health.SetColor(Color.Green);
        }

        public bool IsSupportive()
        {
            return true;
        }

        public string GetName()
        {
            return "Heal";
        }

        public void Update(BattleContainer battle, GameTime gameTime, Input input)
        {
            battle.PushSplashText($"{source.Name} used {GetName()}");

            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            target.Health.CurrentHealth = MathHelper.SmoothStep(beginHP, targetHP, (elapsedTime / (float)eventTime));

            if (HasCompleted())
            {
                target.Health.CurrentHealth = targetHP;
                target.Health.SetColor(Color.White);
            }
        }

        public void Draw(BattleContainer battle, SpriteBatch spritebatch, SpriteFont font)
        {
            spritebatch.DrawString(font, heal.ToString(), target.Position + new Vector2(0, -(elapsedTime / (float)eventTime) * 50 + 20), Color.White, 0, new Vector2(), 2, SpriteEffects.None, 1);
        }

        public bool HasCompleted()
        {
            return elapsedTime >= eventTime;
        }
    }
}
