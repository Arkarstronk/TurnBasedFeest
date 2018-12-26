using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;
using TurnBasedFeest.GameEvents;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.UI
{
    // Let heroes rest and recover their health.
    class RestScreen : UIScreen
    {
        private Game1 game;
        private List<Actor> heroes;
        private MultipleChoiceUI choice;

        public RestScreen(Game1 game, List<Actor> heroes)
        {            
            this.game = game;
            this.heroes = heroes;
            this.choice = new MultipleChoiceUI(new List<string> { "Rest", "Explore area" });
        }

        public void Initialize()
        {            
        }
        
        public void Update(GameTime gameTime, Input input)
        {
            int choiceIndex = choice.Update(gameTime, input);
            if (choiceIndex >= 0)
            {
                switch (choiceIndex)
                {
                    case 0:
                        heroes.ForEach(x => {
                            x.Health.CurrentHealth = x.Health.MaxHealth;
                        });
                        game.SetUIScreen(new WelcomeScreen(game));
                        break;
                    case 1:
                        // get some loot or exp with some chance
                        game.SetUIScreen(new WelcomeScreen(game));
                        break;
                }                
            }
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            choice.Draw(spritebatch, font, new Vector2(Game1.screenWidth * 0.45f, Game1.screenHeight * 0.4f));
            spritebatch.DrawString(font, "You discovered a quiet open place, choose what to do.", new Vector2(0.3f * Game1.screenWidth, 0.7f * Game1.screenHeight), Color.White);

        }
    }
}
