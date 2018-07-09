using System;
using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleSystem;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework.Graphics;

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

        public void SetActors(Actor source, Actor target)
        {
            this.source = source;
            this.target = target;
        }

        public void Initialize()
        {
            elapsedTime = 0;
            beginHP = source.health.actorCurrentHealth;
            targetHP = (source.health.actorCurrentHealth + heal >= source.health.actorMaxHealth) ? source.health.actorMaxHealth : (source.health.actorCurrentHealth + heal);
        }

        public bool Update(Battle battle, Input input)
        {
            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            source.health.actorCurrentHealth = (elapsedTime >= actionTime) ? targetHP : (int) MathHelper.SmoothStep(beginHP, targetHP, (elapsedTime / (float)actionTime));

            if (source.health.actorCurrentHealth == targetHP)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public string GetName()
        {
            return "Heal";
        }

        public void Draw(Battle battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }
}
