using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.GameEvents.Battle;
using TurnBasedFeest.Attributes;
using System;

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
        int damage;

        private string status;

        public void SetActors(Actor source, Actor target)
        {
            this.source = source;
            this.target = target;
        }

        public void Initialize()
        {
            beginHP = (int)target.health.CurrentHealth;
            elapsedTime = 0;            

            int attack = source.GetStats()[StatisticAttribute.ATTACK];
            int defence = target.GetStats()[StatisticAttribute.DEFENCE];
            int sourceSpeed = source.GetStats()[StatisticAttribute.SPEED];
            int targetSpeed = target.GetStats()[StatisticAttribute.SPEED];


            foreach (IAttribute attribute in target.attributes.FindAll(x => x.GetAttributeType() == attributeType.INCOMING))
            {
                defence = (int) (defence * attribute.GetMultiplier()) + (int)attribute.GetAddition();
            }

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
                hitChance = ((difference) / 140.0) * 0.87 + 0.6;
                Console.WriteLine($"hit: {hitChance}, diff: {difference}");
            }

            
            if (Game1.rnd.NextDouble() <= hitChance)
            {
                isCritical = Game1.rnd.NextDouble() >= 0.9;

                if (isCritical)
                {
                    status = $"{source.name} used {GetName()}, Critical!";
                    attack = (int)((damage + 1) * 1.5);
                } else
                {
                    status = $"{source.name} used {GetName()}";
                }

                damage = Math.Max(1, attack - defence);
                
            } else
            {
                status = $"{source.name} used {GetName()} and missed!";
            }

            targetHP = (int) ((target.health.CurrentHealth - damage <= 0) ? 0 : (target.health.CurrentHealth - damage));
            target.health.SetColor(Color.Yellow);
        }

        public bool Update(BattleTurnEvent battle, Input input)
        {            
            battle.PushTextUpdate(status);
            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            target.health.CurrentHealth = MathHelper.SmoothStep(beginHP, targetHP, (elapsedTime / (float)eventTime));

            if (elapsedTime >= eventTime)
            {
                target.health.SetColor(Color.White);                
                target.health.CurrentHealth = targetHP;

                //if this attack killed its target
                if (target.health.CurrentHealth == 0)
                {
                    battle.CurrentActor.battleEvents.Insert(battle.eventIndex + 1, new DeathEvent(target));
                }                

                battle.CurrentActor.battleEvents.RemoveAt(battle.eventIndex);
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

        public void Draw(BattleTurnEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
            spritebatch.DrawString(font, damage.ToString(), target.position + new Vector2(0, -(elapsedTime / (float) eventTime) * 50 + 20), Color.White, 0, new Vector2(), 2, SpriteEffects.None, 1);
        }

        public bool IsSupportive()
        {
            return false;
        }
    }
}
