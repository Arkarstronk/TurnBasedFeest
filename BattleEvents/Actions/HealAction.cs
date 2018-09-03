using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.BattleEvents.Battle;
using TurnBasedFeest.Graphics;

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

        public void SetActors(Actor source, params Actor[] target)
        {
            this.source = source;
            this.target = target[0];
        }

        public void Initialize(BattleContainer battle)
        {
            elapsedTime = 0;
            heal = source.GetStats()[StatisticAttribute.SUPPORT_MAGIC];
            beginHP = (int)target.Health.CurrentHealth;
            targetHP = (int)((target.Health.CurrentHealth + heal >= target.Health.MaxHealth) ? target.Health.MaxHealth : (target.Health.CurrentHealth + heal));            
            target.Health.SetColor(Color.Green);
            battle.ParticleHelper.Add(new TextParticle($"+{targetHP - beginHP}", 1200, target.Position, new Vector2(30.0f, -30.0f)));
        }

        public string GetName() => "Heal";
        public ActionTarget GetTarget() => new ActionTarget(ActionTarget.TargetSide.FRIENDLY);



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
        }

        public bool HasCompleted()
        {
            return elapsedTime >= eventTime;
        }
    }
}
