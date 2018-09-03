using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;
using TurnBasedFeest.GameEvents;
using TurnBasedFeest.Graphics;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.UI
{
    class BattleScreen : UIScreen
    {
        private Game1 game;
        private ParticleHelper particleHelper;
        private BattleContainer battle;
        private Texture2D background;

        public BattleScreen(Game1 game, List<Actor> actors)
        {
            this.game = game;
            this.particleHelper = new ParticleHelper();
            this.battle = BattleContainer.CreateBattle(this.particleHelper, actors);
            this.background = TextureFactory.Instance.GetTexture("background");            
        }

        public void Initialize()
        {
            Console.WriteLine("Initialized battle");
        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            batch.Draw(background, new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), new Color(0.1f, 0.1f, 0.2f));
            battle.Draw(batch, font);
            particleHelper.Draw(batch, font);
        }

        public void Update(GameTime gameTime, Input input)
        {
            // Update everything regarding the battle
            battle.Update(gameTime, input);
            particleHelper.Update(gameTime);

            // Finally, if heroes are dead, end the battle.
            var victors = battle.GetVictors();
            if (victors != BattleContainer.Victors.NONE)
            {
                EndBattle(victors);
            }
        }

        private void EndBattle(BattleContainer.Victors victors)
        {            
            if (victors == BattleContainer.Victors.ENEMY)
            {
                game.Exit();
            } else
            {
                int experienceGained = calculateGainedExperience();
                var alivePlayers = battle.GetAliveActors().FindAll(x => x.IsPlayer());

                List<ExperienceResult> results = new List<ExperienceResult>();

                alivePlayers.ForEach(x => {
                    var result = x.GetStats().AddExperience(experienceGained / alivePlayers.Count, ((PlayerInfo)x.Info).GetLevelingScheme());

                    if (result.LeveledUp)
                    {
                        Console.WriteLine($"{x.Name} Leveled up to {result.NewLevel}");

                        x.Health.MaxHealth = x.GetStats().MaxHealth;
                    }

                    results.Add(result);
                });
                Console.WriteLine($"Gained {experienceGained}");

                battle.EndBattle();
                game.SetUIScreen(new ExperienceScreen(game, alivePlayers, results));
            }
        }

        private int calculateGainedExperience()
        {
            var enemyActors = battle.GetActors().FindAll(x => !x.IsPlayer());
            return enemyActors.Sum(x => ((EnemyInfo)x.Info).GetXP());
        }
    }
}
