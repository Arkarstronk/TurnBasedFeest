using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleEvents.Actions;
using TurnBasedFeest.GameEvents.Battle;

namespace TurnBasedFeest.BattleEvents.TurnBehaviour
{
    // Randomly does positive things (from the perspective of the actor) 
    class EfficientRandomAI : ITurnEvent
    {
        public void Initialize()
        {

        }

        public bool Update(BattleTurnEvent battle, Input input)
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

            battle.currentActor.battleEvents.Insert(battle.eventIndex + 1, randomAction);

            return true;
        }

        public void Draw(BattleTurnEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }
}
