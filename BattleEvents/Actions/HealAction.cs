﻿using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.GameEvents;

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
        int heal = 20;

        public void SetActors(Actor source, Actor target)
        {
            this.source = source;
            this.target = target;
        }

        public void Initialize()
        {
            elapsedTime = 0;
            beginHP = (int)source.health.CurrentHealth;
            targetHP = (int)((source.health.CurrentHealth + heal >= source.health.MaxHealth) ? source.health.MaxHealth : (source.health.CurrentHealth + heal));
        }

        public bool Update(BattleEvent battle, Input input)
        {
            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            source.health.CurrentHealth = (elapsedTime >= eventTime) ? targetHP : MathHelper.SmoothStep(beginHP, targetHP, (elapsedTime / (float)eventTime));

            if (elapsedTime >= eventTime)
            {
                battle.currentActor.battleEvents.RemoveAt(battle.eventIndex);
                battle.eventIndex--;
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool isPositive()
        {
            return true;
        }

        public string GetName()
        {
            return "Heal";
        }

        public void Draw(BattleEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }
}
