using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actions;
using TurnBasedFeest.BattleSystem;
using System;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Events.TurnBehaviour;

namespace TurnBasedFeest.Actors.Behaviours
{
    class RandomAIEvent : ITurnBehaviourEvent
    {
        public void Initialize()
        {

        }

        public bool Update(Battle battle, Input input)
        {
            IAction randomAction = battle.currentActor.actions[Game1.rnd.Next(battle.currentActor.actions.Count)];
            //needs to exclude enemies somehow
            List<Actor> possibleTargets = battle.actors.FindAll(x => true);
            Actor randomActor = possibleTargets[Game1.rnd.Next(possibleTargets.Count)];
            randomAction.SetActors(battle.currentActor, randomActor);

            int index = battle.currentActor.battleEvents.IndexOf(this);
            battle.currentActor.battleEvents.Insert(index + 1, randomAction);

            return true;
        } 

        public void Draw(Battle battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }    
}
