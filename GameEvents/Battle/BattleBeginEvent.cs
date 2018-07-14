using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Utilities;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using Microsoft.Xna.Framework;

namespace TurnBasedFeest.GameEvents.Battle
{
    class BattleBeginEvent : IGameEvent
    {
        BattleEvent battle;
        int eventTime = 2000;
        int elapsedTime = 0;

        public BattleBeginEvent(BattleEvent battle)
        {
            this.battle = battle;
        }

        public void Initialize(List<Actor> actors)
        {
            int parseDistanceAlly = (int)((0.5f * Game1.screenHeight) / (actors.FindAll(x => x.isPlayer).Count + 1));
            for (int i = 0; i < actors.FindAll(x => x.isPlayer).Count; i++)
            {
                actors.FindAll(x => x.isPlayer)[i].position = new Vector2(Game1.screenWidth - 50 - 0.8f * Game1.screenWidth, 0.1f * Game1.screenHeight + (i + 1) * parseDistanceAlly);
            }

            int parseDistanceEnemies = (int)((0.5f * Game1.screenHeight) / (actors.FindAll(x => !x.isPlayer).Count + 1));
            for (int i = 0; i < actors.FindAll(x => !x.isPlayer).Count; i++)
            {
                actors.FindAll(x => !x.isPlayer)[i].position = new Vector2(Game1.screenWidth - 50 - 0.2f * Game1.screenWidth, 0.1f * Game1.screenHeight + (i + 1) * parseDistanceEnemies);
            }
        }

        public bool Update(Game1 game, Input input)
        {
            battle.battleText = "A new battle started!";

            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= eventTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }
}
