using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework;
using System.Linq;

namespace TurnBasedFeest.BattleSystem
{
    class Battle
    {
        public bool ongoingBattle;
        public List<Actor> actors;
        public List<Actor> aliveActors;
        public Actor currentActor;
        public int eventIndex;

        public void InitializeFight(List<Actor> actors)
        {
            ongoingBattle = true;
            this.actors = actors;
            this.aliveActors = this.actors;
            actors.ForEach(x => x.Initialize());
            eventIndex = 0;
            currentActor = this.actors[0];
            currentActor.hasTurn = false;
            currentActor.battleEvents[eventIndex].Initialize();
        }

        public void EndFight()
        {
            ongoingBattle = false;
        }
        
        public BattleResult Update(Input input)
        {            
            // if the current event exists
            if (eventIndex < currentActor.battleEvents.Count)
            {
                // preform the event AND if the current event is done
                if (currentActor.battleEvents[eventIndex].Update(this, input))
                {
                    // go to the next event
                    eventIndex++;
                    // and initialize it if it exists
                    if (eventIndex < currentActor.battleEvents.Count)
                    {
                        currentActor.battleEvents[eventIndex].Initialize();
                    }
                }
            }
            // if the event does not exist go to the next actor
            else
            {
                currentActor = getNextActor();
            }

            aliveActors = actors.FindAll(x => x.health.actorCurrentHealth > 0);
            actors.ForEach(x => x.Update());

            // if the current player dies during his turn
            if (currentActor.health.actorCurrentHealth <= 0)
            {
                currentActor = getNextActor();
            }

            // if there are no more alive players
            if (aliveActors.Count == 0)
            {
                return new BattleResult(true, false, aliveActors);
            }
            else
            {
                // if there are only players alive
                if (aliveActors.TrueForAll(x => x.isPlayer))
                {
                    return new BattleResult(true, true, aliveActors);
                }
                // if there are only enemies alive
                if (aliveActors.TrueForAll(x => !x.isPlayer))
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
                if (actor.health.actorCurrentHealth > 0)
                {
                    actor.Draw(spritebatch, font);
                }
            }

            for (int i = 0; i < currentActor.battleEvents.Count; i++)
            {
                spritebatch.DrawString(font, currentActor.battleEvents[i].ToString(), new Vector2(0, 15 * i), i == eventIndex ? Color.Red: Color.White);
            }

            if (eventIndex < currentActor.battleEvents.Count)
            {
                currentActor.battleEvents[eventIndex].Draw(this, spritebatch, font);
            }
            spritebatch.DrawString(font, ">", currentActor.position - new Vector2(35, 0), Color.White);
        }

        private Actor getNextActor()
        {
            // if all the actors are dead (battle will end in this same update loop)
            if (aliveActors.Count == 0)
            {
                return null;
            }
            else
            {
                List<Actor> actorsWithTurn = aliveActors.FindAll(x => x.hasTurn);

                // if all the actors have done their turn
                if(actorsWithTurn.Count == 0)
                {
                    aliveActors.ForEach(x => x.Initialize());
                    eventIndex = 0;
                    aliveActors[0].hasTurn = false;
                    aliveActors[0].battleEvents[eventIndex].Initialize();
                    return aliveActors[0];
                }
                else
                {
                    eventIndex = 0;
                    actorsWithTurn[0].hasTurn = false;
                    actorsWithTurn[0].battleEvents[eventIndex].Initialize();
                    return actorsWithTurn[0];
                }
            }                   
        }
    }
}
