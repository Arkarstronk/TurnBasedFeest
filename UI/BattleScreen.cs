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
    class BattleScreen : UIScreen
    {
        private Game1 game;
        
        private BattleContainer battle;

        public BattleScreen(Game1 game, List<Actor> actors)
        {
            this.game = game;        
            this.battle = BattleContainer.CreateBattle(actors);
        }

        public void Initialize()
        {
            Console.WriteLine("Initialized battle");
        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            battle.Draw(batch, font);
        }

        public void Update(GameTime gameTime, Input input)
        {
            // Update everything regarding the battle
            battle.Update(gameTime, input);


            // Finally, if heroes are dead, end the battle.
            var victors = battle.GetVictors();
            if (victors != BattleContainer.Victors.NONE)
            {
                EndBattle(victors);
            }
        }

        private void EndBattle(BattleContainer.Victors victors)
        {
            battle.EndBattle();
            if (victors == BattleContainer.Victors.ENEMY)
            {
                game.Exit();
            }
            game.SetUIScreen(new WelcomeScreen(game));
        }
    }
}
