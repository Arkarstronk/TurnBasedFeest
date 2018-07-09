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
        public List<Actor> actors = new List<Actor>();
        List<Actor>.Enumerator actorEnum;
        public Actor currentActor;
        public int eventIndex;

        public void InitializeFight(List<Actor> actors)
        {
            this.actors = actors;
            actorEnum = this.actors.GetEnumerator();
            currentActor = getNextActor();
            ongoingBattle = true;
        }

        public void EndFight()
        {
            ongoingBattle = false;
        }
        
        public BattleResult Update(Input input)
        {
            if (eventIndex < currentActor.battleEvents.Count)
            {
                if (currentActor.battleEvents[eventIndex].Update(this, input))
                {
                    eventIndex++;
                    if (eventIndex < currentActor.battleEvents.Count)
                    {
                        currentActor.battleEvents[eventIndex].Initialize();
                    }
                }
            }
            else
            {
                currentActor = getNextActor();
            }


            actors.ForEach(x => x.Update());

            List<Actor> aliveActors = actors.FindAll(x => x.health.actorCurrentHealth > 0);

            if (aliveActors.Count == 0)
            {
                return new BattleResult(true, false, aliveActors);
            }
            /*
            else
            {
                if (aliveActors.TrueForAll(allemaal players))
                {
                    return new BattleResult(true, true, aliveActors);
                }
                if (aliveActors.TrueForAll(allemaal enemies))
                {
                    return new BattleResult(true, false, aliveActors);
                }
            }
            */
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

            if (eventIndex < currentActor.battleEvents.Count)
            {
                currentActor.battleEvents[eventIndex].Draw(this, spritebatch, font);
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
                    eventIndex = 0;
                    actorEnum.Current.battleEvents[eventIndex].Initialize();
                    return actorEnum.Current;
                }
            }                      
        }
    }
}
