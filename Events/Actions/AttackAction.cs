﻿using System;
using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleSystem;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedFeest.Events.Actions
{
    class AttackAction : IAction
    {
        int actionTime = 1000;
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

        public bool Update(Battle battle, Input input)
        {
            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            target.health.actorCurrentHealth = (elapsedTime >= actionTime) ? targetHP : MathHelper.SmoothStep(beginHP, targetHP, (elapsedTime / (float)actionTime));

            if (elapsedTime >= actionTime)
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

        public string GetName()
        {
            return "Attack";
        }

        public void Draw(Battle battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }
}