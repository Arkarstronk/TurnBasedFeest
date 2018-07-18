using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using Microsoft.Xna.Framework;

namespace TurnBasedFeest.GameEvents.Battle
{
    class BattleEvent : IGameEvent
    {
        public List<Actor> actors;
        public List<Actor> aliveActors;
        List<IGameEvent> events;
        int eventIndex = 0;
        public string battleText = string.Empty;

        public void Initialize(List<Actor> actors)
        {
            events = new List<IGameEvent> { new BattleBeginEvent(this), new BattleTurnEvent(this), new BattleEndEvent(this) };
            this.actors = actors;
            //we want the position of all actors to be considered, even dead alies
            events[0].Initialize(actors);
            aliveActors = new List<Actor>(actors.FindAll(x => x.health.CurrentHealth > 0));
        }

        public bool Update(Game1 game, Input input)
        {
            // preform the event AND if the current event is done
            if (events[eventIndex].Update(game, input))
            {
                // go to the next event
                eventIndex++;
                // and initialize it if it exists
                if (eventIndex < events.Count)
                {
                    events[eventIndex].Initialize(aliveActors);
                }
                // if it does not exist, we are done
                else
                {
                    game.eventCounter++;
                    return true;
                }
            }

            return false;
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            //draws the battle text UI
            spritebatch.DrawString(font, battleText, new Vector2(0.2f * Game1.screenWidth, 0.75f * Game1.screenHeight), Color.White);

            //draw actors
            foreach (Actor actor in actors)
            {
                actor.Draw(spritebatch, font);
            }

            //draw current event
            if (eventIndex < events.Count)
            {
                events[eventIndex].Draw(spritebatch, font);
            }

            //debug
            for (int i = 0; i < events.Count; i++)
            {
                spritebatch.DrawString(font, events[i].ToString(), new Vector2(0, 15 * i), i == eventIndex ? Color.Red : Color.White);
            }
        }     
    }
}
