using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Utilities;
using System.Collections.Generic;
using TurnBasedFeest.Actors;

namespace TurnBasedFeest.GameEvents.Battle
{
    class BattleBeginEvent : IGameEvent
    {
        int eventTime = 2000;
        int elapsedTime = 0;

        
        public void Initialize(List<Actor> actors)
        {
        }    

        public bool Update(Game1 game, Input input)
        {
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
