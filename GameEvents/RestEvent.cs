using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.GameEvents.UI;
using Microsoft.Xna.Framework;

namespace TurnBasedFeest.GameEvents
{
    class RestEvent : IGameEvent
    {
        List<Actor> actors;
        MultipleChoiceEvent choice;

        public void Initialize(List<Actor> actors)
        {
            choice = new MultipleChoiceEvent(new List<string> { "Rest", "Explore area" });
            this.actors = actors;
        }

        public bool Update(Game1 game, Input input)
        {
            int choiceIndex = choice.Update(game, input);
            if (choiceIndex >= 0)
            {
                switch (choiceIndex)
                {
                    case 0:
                        actors.ForEach(x => x.health.CurrentHealth = x.health.MaxHealth);
                        break;
                    case 1:
                        // get some loot or exp with some chance
                        break;
                }
                game.nextEvent = new EventDeterminerEvent(game);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            choice.Draw(spritebatch, font, new Vector2(Game1.screenWidth * 0.45f, Game1.screenHeight * 0.4f));
            spritebatch.DrawString(font, "You discovered a quiet open place, choose what to do.", new Vector2(0.3f * Game1.screenWidth, 0.7f * Game1.screenHeight), Color.White);

        }
    }
}
