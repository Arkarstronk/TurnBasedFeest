using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors.Behaviours;
using TurnBasedFeest.Actions;
using System.Linq;

namespace TurnBasedFeest.BattleSystem
{
    class Battle
    {
        public bool ongoingBattle;
        List<Actor> actors = new List<Actor>();
        List<Actor>.Enumerator actorEnum;
        Actor currentActor;

        public void InitializeFight(List<Actor> actors)
        {
            this.actors = actors;
            actorEnum = this.actors.GetEnumerator();
            actorEnum.MoveNext();
            currentActor = actorEnum.Current;
            ongoingBattle = true;
        }

        public void EndFight()
        {
            ongoingBattle = false;
        }


        public BattleResult Update(Input input)
        {
            // If a behaviour is determined
            if(currentActor.turnBehaviour.DetermineBehaviour(input, actors.FindAll(x => x.health.actorCurrentHealth > 0), currentActor))
            {
                ITurnResult turnResult = currentActor.turnBehaviour.GetTurnResult();
                IActionResult actionResult = turnResult.Preform(currentActor);
                currentActor = getNextActor();
            }

            actors.ForEach(x => x.Update());

            List<Actor> aliveActors = actors.FindAll(x => x.health.actorCurrentHealth > 0);

            if(aliveActors.Count == 0)
            {
                return new BattleResult(true, false, aliveActors);
            }
            else
            {
                if (aliveActors.TrueForAll(x => x.turnBehaviour.GetType() == typeof(PlayerTurnBehaviour)))
                {
                    return new BattleResult(true, true, aliveActors);
                }
                if (aliveActors.TrueForAll(x => x.turnBehaviour.GetType() != typeof(PlayerTurnBehaviour)))
                {
                    return new BattleResult(true, false, aliveActors);
                }
            }

            return new BattleResult(false, false, aliveActors);
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            foreach (Actor actor in actors)
            {
                if(actor.health.actorCurrentHealth > 0)
                {
                    actor.Draw(spritebatch, font);
                }                
            }

            spritebatch.DrawString(font, ">", currentActor.position - new Vector2(35, 0), Color.White);
        }

        private Actor getNextActor()
        {
            if (actors.Count(x => x.health.actorCurrentHealth > 0) == 0)
            {
                return currentActor;
            }
            while (true)
            {
                if (!actorEnum.MoveNext())
                {
                    actorEnum = actors.GetEnumerator();
                    actorEnum.MoveNext();
                }
                if (actorEnum.Current.health.actorCurrentHealth > 0)
                {
                    return actorEnum.Current;
                }
            }                      
        }
    }
}
