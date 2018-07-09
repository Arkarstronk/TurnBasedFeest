using System;
using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleSystem;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.Actions
{
    class ActionDefend : IAction
    {
        int actionTime = 1000;
        int elapsedTime;
        Actor source;
        Actor target;

        public void SetActors(Actor source, Actor target)
        {
            this.source = source;
            this.target = target;
        }

        public void Initialize()
        {
            elapsedTime = 0;
            this.source.health.color = Color.Violet;
        }

        public bool Update(Battle battle, Input input)
        {
            elapsedTime += (int) Game1.time.ElapsedGameTime.TotalMilliseconds;
            
            if(elapsedTime > actionTime)
            {
                source.health.color = Color.White;
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
            return "Defend";
        }

        public void Draw(Battle battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }
}
