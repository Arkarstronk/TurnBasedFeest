﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.GameEvents;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Actors;
using Microsoft.Xna.Framework;

namespace TurnBasedFeest.BattleEvents
{
    class DeathEvent : IBattleEvent
    {
        int eventTime = 1000;
        int elapsedTime;
        Actor deceased;

        public DeathEvent(Actor deceased)
        {
            this.deceased = deceased;
        }

        public void Initialize()
        {
            elapsedTime = 0;
            deceased.color = Color.Red;
        }

        public bool Update(BattleEvent battle, Input input)
        {
            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= eventTime)
            {
                // potentially an extreme small amount of chance to cause an innocent bug if actors do not have to be unique
                battle.aliveActors.Remove(deceased);

                // if the current player dies during his turn
                if (battle.currentActor == deceased)
                {
                    battle.currentActor = battle.getNextActor();
                }

                deceased.color = Color.White;

                battle.currentActor.battleEvents.RemoveAt(battle.eventIndex);
                battle.eventIndex--;

                return true;
            }

            return false;
        }

        public void Draw(BattleEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }

    }
}