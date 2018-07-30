using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework;

namespace TurnBasedFeest.GameEvents.Battle
{
    class BattleTurnEvent : IGameEvent
    {
        private BattleEvent battle;
        public List<Actor> allActors;
        public List<Actor> aliveActors;
        public Actor CurrentActor;
        public int eventIndex;

        public BattleTurnEvent(BattleEvent battle)
        {
            this.battle = battle;
        }

        public void Initialize(List<Actor> aliveActors)
        {
            allActors = new List<Actor>(aliveActors);
            this.aliveActors = aliveActors;
            this.aliveActors.ForEach(x => x.Initialize());
            CurrentActor = getNextActor();
        }

        public bool Update(Game1 game, Input input)
        {
            // preform the event AND if the current event is done
            if (CurrentActor.battleEvents[eventIndex].Update(this, input))
            {
                // go to the next event
                eventIndex++;
                // and initialize it if it exists
                if (eventIndex < CurrentActor.battleEvents.Count)
                {
                    CurrentActor.battleEvents[eventIndex].Initialize();
                }
                // if it does not exist, go to the next actor
                else
                {
                    CurrentActor = getNextActor();
                }
            }

            // update the alive actors
            aliveActors.ForEach(x => x.Update());

            // if there are no more alive players
            if (aliveActors.Count == 0 || aliveActors.TrueForAll(x => x.isPlayer) || aliveActors.TrueForAll(x => !x.isPlayer))
            {
                battle.aliveActors = aliveActors;
                return true;
            }
            return false;
        }

        public void PushTextUpdate(string text)
        {
            this.battle.battleText = text;
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            //draws debug
            for (int i = 0; i < CurrentActor.battleEvents.Count; i++)
            {
                spritebatch.DrawString(font, CurrentActor.battleEvents[i].ToString(), new Vector2(Game1.screenWidth - 400, 15 * i), i == eventIndex ? Color.Red: Color.White);
            }

            //draws the current event
            if (eventIndex < CurrentActor.battleEvents.Count)
            {
                CurrentActor.battleEvents[eventIndex].Draw(this, spritebatch, font);
            }

            //draws an indication of the current player
            spritebatch.DrawString(font, ">", CurrentActor.position - new Vector2(35, 0), Color.White);
        }

        public Actor getNextActor()
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
