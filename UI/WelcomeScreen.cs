using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;
using System.Linq;
using TurnBasedFeest.BattleEvents.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TurnBasedFeest.Graphics;
using TurnBasedFeest.UI;
using TurnBasedFeest.BattleEvents.Battle;
using TurnBasedFeest.BattleEvents.Gorilla;
using TurnBasedFeest.Actors.npcs;

namespace TurnBasedFeest.GameEvents
{
    class WelcomeScreen : UIScreen
    {
        UIScreen nextScreen;
        Game1 game;

        public WelcomeScreen(Game1 game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            if(game.hardcodedEvents.Where(x => x.Key <= game.eventCounter).ToList().Count > 0)
            {
                int hardcodedEventKey = game.hardcodedEvents.Where(x => x.Key <= game.eventCounter).OrderByDescending(x => x.Key).Last().Key;
                string hardcodedEventString = game.hardcodedEvents[hardcodedEventKey];
                game.hardcodedEvents.Remove(hardcodedEventKey);
                SetHardcodedEvent(hardcodedEventString);
            }
            else
            {
                setRandomEvent();
            }
        }

        public void Update(GameTime gameTime, Input input)
        {
            if (input.Pressed(Keys.Enter))
            {
                game.SetUIScreen(nextScreen);                
            }
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            //draws some text for now
            spritebatch.DrawString(font, "Press enter to go to the next event!", new Vector2(0.4f * Game1.screenWidth, 0.5f * Game1.screenHeight), Color.White);

            //debugs the hardcoded events
            for(int i = 0; i < game.hardcodedEvents.Count; i++)
            {
                spritebatch.DrawString(font, $"{game.hardcodedEvents.ElementAt(i).Key.ToString()}  =>  {game.hardcodedEvents.ElementAt(i).Value.ToString()}", new Vector2(0, 15 * i), Color.Red);
            }

            spritebatch.DrawString(font, $"Turn count : {game.eventCounter.ToString()}", new Vector2(Game1.screenWidth * 0.45f, 0), Color.Red);

        }

        private void setRandomEvent()
        {
            int random = Game1.rnd.Next(100);

            // With 20% probability, start a boss battle.
            if (random <= 20)
            {
                List<Actor> actors = new List<Actor>();
                actors.AddRange(game.heroes);
                actors.Add(new BattleGorilla().Create(0));
                
                nextScreen = new BattleScreen(game, actors);
            }
            else if (random <= 80)
            {
                // With 50% probability, start a boss battle.
                List<Actor> actors = new List<Actor>();
                actors.AddRange(game.heroes);
                int monkeys = Game1.rnd.Next(2, 5);
                for (int i = 0; i < monkeys  + 1; i++)
                {
                    actors.Add(new BattleMonkey_1().Create(i));
                }
                for (int i = 0; i < monkeys + 1; i++)
                {
                    actors.Add(new Platipus_1().Create(i));
                }

                nextScreen = new BattleScreen(game, actors);
            }
            else
            {
                nextScreen = new RestScreen(game, game.heroes);
            }
        }

        private void SetHardcodedEvent(string hardcodedEvent)
        {
            switch (hardcodedEvent)
            {
                case "bossbattle1":
                    break;
            }
        } 

       
    }
}
