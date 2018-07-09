using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.BattleSystem;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Events.Actions;
using TurnBasedFeest.Actors;

namespace TurnBasedFeest.Events.TurnBehaviour
{
    // Randomly does positive things (from the perspective of the actor) 
    class EfficientRandomAI : IBattleEvent
    {
        public void Initialize()
        {

        }

        public bool Update(Battle battle, Input input)
        {
            IAction randomAction = battle.currentActor.actions[Game1.rnd.Next(battle.currentActor.actions.Count)];

            List<Actor> possibleTargets;
            if (randomAction.isPositive())
            {
                possibleTargets = battle.aliveActors.FindAll(x => x.isPlayer == battle.currentActor.isPlayer);
            }
            else
            {
                possibleTargets = battle.aliveActors.FindAll(x => x.isPlayer != battle.currentActor.isPlayer);
            }

            Actor randomActor = possibleTargets[Game1.rnd.Next(possibleTargets.Count)];
            randomAction.SetActors(battle.currentActor, randomActor);

            int index = battle.eventIndex;
            battle.currentActor.battleEvents.Insert(index + 1, randomAction);

            return true;
        }

        public void Draw(Battle battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }
}
