using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Actions;
using System.Linq;

namespace TurnBasedFeest.Actors.Behaviours
{
    class RandomEnemyTurnBehaviour : ITurnBehaviour
    {
        RandomEnemyTurnResult result;

        public void Initialize()
        {

        }

        public bool DetermineBehaviour(Input input, List<Actor> actors, Actor currentActor)
        {
            IAction randomAction = currentActor.actions[Game1.rnd.Next(currentActor.actions.Count)];
            List<Actor> possibleTargets = actors.FindAll(x => x.turnBehaviour.GetType() == typeof(PlayerTurnBehaviour));
            Actor randomActor = possibleTargets[Game1.rnd.Next(possibleTargets.Count)];
            result = new RandomEnemyTurnResult(randomAction, randomActor, currentActor);
            return true;
        }        

        public ITurnResult GetTurnResult()
        {
            return result;
        }

        public void Draw(SpriteFont font, SpriteBatch spritebatch)
        {
        }
    }

    class RandomEnemyTurnResult : ITurnResult
    {
        IAction resultAction;
        Actor targetActor;
        Actor sourceActor;

        public RandomEnemyTurnResult(IAction action, Actor target, Actor source)
        {
            this.resultAction = action;
            sourceActor = source;
            targetActor = target;
        }

        public void Initialize()
        {
            resultAction.Initialize(sourceActor, targetActor);
        }

        public IActionResult Update()
        {
            return resultAction.Update();
        }
    }
}
