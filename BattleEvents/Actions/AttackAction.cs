﻿using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Attributes;
using System;

namespace TurnBasedFeest.BattleEvents.Actions
{
    class AttackAction : IAction, ContinueableAction
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

        public void SetActors(Actor source, Actor target)
        {
            this.source = source;
            this.target = target;
        }

        public void Initialize()
        {
            beginHP = (int)target.Health.CurrentHealth;
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
                target.Health.Shake = true;
                isCritical = Game1.rnd.NextDouble() >= 0.9;

                if (isCritical)
                {
                    status = $"{source.Name} used {GetName()}, Critical!";
                    attack = (int)((damage + 1) * 1.5);
                } else
                {
                    status = $"{source.Name} used {GetName()}";
                }

                damage = Math.Max(1, attack - defence);
                
            } else
            {
                status = $"{source.Name} used {GetName()} and missed!";
            }

            targetHP = (int) ((target.Health.CurrentHealth - damage <= 0) ? 0 : (target.Health.CurrentHealth - damage));
            target.Health.SetColor(Color.DarkRed);
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

        public string GetName()
        {
            return "Attack";
        }        

        public bool IsSupportive()
        {
            return false;
        }

        public void Draw(BattleContainer battle, SpriteBatch spritebatch, SpriteFont font)
        {
            spritebatch.DrawString(font, damage.ToString(), target.Position + new Vector2(0, -(elapsedTime / (float)eventTime) * 50 + 20), Color.White, 0, new Vector2(), 2, SpriteEffects.None, 1);
        }

        public bool HasCompleted()
        {
            return elapsedTime >= eventTime;
        }

        public bool HasNextEvent()
        {
            return nextEvent != null;
        }
        public ITurnEvent NextEvent()
        {
            return nextEvent;
        }
    }
}
