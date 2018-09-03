using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Attributes;
using System;
using TurnBasedFeest.BattleEvents.Battle;
using TurnBasedFeest.Graphics;

namespace TurnBasedFeest.BattleEvents.Actions
{
    class AttackAction : IAction, IContinueableAction
    {
        int eventTime = 1000;
        int elapsedTime;
        Actor source;
        Actor target;
        int beginHP;
        int targetHP;
        int damage;


        private ITurnEvent nextEvent = null;
        private string status;

        public void SetActors(Actor source, params Actor[] target)
        {
            this.source = source;
            this.target = target[0];
        }

        public void Initialize(BattleContainer battle)
        {
            beginHP = (int)target.Health.CurrentHealth;
            elapsedTime = 0;
            damage = 0;

            int attack = source.GetStats()[StatisticAttribute.ATTACK, source.Attributes];            
            int sourceSpeed = source.GetStats()[StatisticAttribute.SPEED, source.Attributes];
            int targetSpeed = target.GetStats()[StatisticAttribute.SPEED, target.Attributes];
            int defence = target.GetStats()[StatisticAttribute.DEFENCE, target.Attributes];            

            // Using attack, defence, source and target speeds.
            // Basically, if the target is too fast it is unlikely to hit. But if the source is way faster, critical!
            //

            double hitChance = 0.0;
            bool isCritical = false;
            int difference = sourceSpeed - targetSpeed;
            

            // If the source is way faster than the target, critical!
            if (difference >= 70)
            {
                hitChance = 0.97;
                isCritical = true;
            } else if (difference <= -70)
            {
                // The target is way faster than the source...
                isCritical = false;
                hitChance = 0.03;
            } else {
                // The difference cap is 70
                // Calculate a chance to hit the target based on a log
                // If the source is faster than target, chance to hit should be higher
                // and vice versa
                hitChance = ((difference) / 140.0) * 0.87 + 0.7;             
            }

            
            if (Game1.rnd.NextDouble() <= hitChance)
            {
                target.Health.Shake = true;
                isCritical = Game1.rnd.NextDouble() >= 0.9;

                if (isCritical)
                {
                    status = $"{source.Name} used {GetName()}, Critical!";
                    attack = (int)((attack + 2) * 1.5);
                } else
                {
                    status = $"{source.Name} used {GetName()}";
                }
                
                damage = Math.Max(1, attack - defence);

                double randomDamageRange = 0.2;
                double damageMultiplier = (Game1.rnd.NextDouble()) * randomDamageRange + (1 - randomDamageRange / 2);
                Console.WriteLine($"Multiplier: {damageMultiplier}");
                damage = Math.Max(1, (int)(damage * damageMultiplier));


                targetHP = (int)((beginHP - damage <= 0) ? 0 : (beginHP - damage));
                target.Health.SetColor(Color.DarkRed);
                battle.ParticleHelper.Add(new TextParticle($"-{damage}", 1200, target.Position, new Vector2(30.0f, -30.0f)));

            } else
            {
                damage = 0;
                status = $"{source.Name} used {GetName()} and missed!";
                target.Health.SetColor(Color.CornflowerBlue);
                targetHP = beginHP;
            }
        }

        public void Update(BattleContainer battle, GameTime gameTime, Input input)        
        {               
            battle.PushSplashText(status);
            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            target.Health.CurrentHealth = MathHelper.SmoothStep(beginHP, targetHP, (elapsedTime / (float)eventTime));

            if (HasCompleted())
            {
                target.Health.SetColor(Color.White);                
                target.Health.CurrentHealth = targetHP;
                target.Health.Shake = false;

                if (!target.IsAlive())
                {
                    nextEvent = new DeathEvent(target);
                }
            }            
        }

        public string GetName() => "Attack";
        public ActionTarget GetTarget() => new ActionTarget(ActionTarget.TargetSide.ENEMY);
        public bool HasCompleted() => elapsedTime >= eventTime;

        public ITurnEvent NextEvent() => nextEvent;
        public bool HasNextEvent() => nextEvent != null;

        public void Draw(BattleContainer battle, SpriteBatch spritebatch, SpriteFont font)
        {
            //spritebatch.DrawString(font, damage.ToString(), target.Position + new Vector2(0, -(elapsedTime / (float)eventTime) * 50 + 20), Color.White, 0, new Vector2(), 2, SpriteEffects.None, 1);
        }


        

        
    }
}
