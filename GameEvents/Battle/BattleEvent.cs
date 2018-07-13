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

        public void Initialize(List<Actor> actors)
        {
            events = new List<IGameEvent> { new BattleBeginEvent(), new BattleTurnEvent(this), new BattleEndEvent(this) };
            this.actors = actors;
            events[1].Initialize(actors);
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
                    events[eventIndex].Initialize(actors);
                }
                // if it does not exist, we are done
                else
                {
                    return true;
                }
            }

            return false;
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            events.ForEach(x => x.Draw(spritebatch, font));

            for (int i = 0; i < events.Count; i++)
            {
                spritebatch.DrawString(font, events[i].ToString(), new Vector2(0, 15 * i), i == eventIndex ? Color.Red : Color.White);
            }
        }     
    }
}
