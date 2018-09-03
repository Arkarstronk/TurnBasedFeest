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
    class ExperienceScreen : UIScreen
    {
        private Game1 game;
        private List<Actor> heroes;
        private List<ExperienceResult> results;
        private MultipleChoiceUI choice;

        public ExperienceScreen(Game1 game, List<Actor> heroes, List<ExperienceResult> results)
        {
            this.game = game;
            this.heroes = heroes;
            this.results = results;
            this.choice = new MultipleChoiceUI(new List<string> { "Continue" });
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
                        game.SetUIScreen(new WelcomeScreen(game));
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            choice.Draw(spritebatch, font, new Vector2(Game1.screenWidth * 0.45f, Game1.screenHeight * 0.2f));
            spritebatch.DrawString(font, "You gained some experience!", new Vector2(0.3f * Game1.screenWidth, 0.7f * Game1.screenHeight), Color.White);

            float lineHeight = 0.4f * Game1.screenHeight;

            for (int i = 0; i < heroes.Count; i++)
            {
                String surfix = (results[i].LeveledUp ? $"Level up! {results[i].Level} -> {results[i].NewLevel}" : "");
                spritebatch.DrawString(font, $"{heroes[i].Name}: {results[i].Experience}, {surfix}", new Vector2(0.3f * Game1.screenWidth, lineHeight), Color.White);
                foreach(var x in results[i].statups)
                {
                    lineHeight += 0.022f * Game1.screenHeight;
                    spritebatch.DrawString(font, $"{x.Key.ToString()}: {x.Value.Item1} -> {x.Value.Item2}", new Vector2(0.31f * Game1.screenWidth, lineHeight), Color.White);

                }
                lineHeight += 0.03f * Game1.screenHeight;
            }

        }
    }
}
