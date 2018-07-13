using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Actors;
using TurnBasedFeest.GameEvents.Battle;
using TurnBasedFeest.BattleEvents.Actions;

namespace TurnBasedFeest.BattleEvents.TurnBehaviour
{
    // Randomly does things without caring if its positive or negative
    class RandomAI : ITurnEvent
    {
        public void Initialize()
        {

        }

        public bool Update(BattleTurnEvent battle, Input input)
        {
            IAction randomAction = battle.currentActor.actions[Game1.rnd.Next(battle.currentActor.actions.Count)];
            List<Actor> possibleTargets = battle.aliveActors;
            Actor randomActor = possibleTargets[Game1.rnd.Next(possibleTargets.Count)];
            randomAction.SetActors(battle.currentActor, randomActor);

            battle.currentActor.battleEvents.Insert(battle.eventIndex + 1, randomAction);

            return true;
        } 

        public void Draw(BattleTurnEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }    
}
