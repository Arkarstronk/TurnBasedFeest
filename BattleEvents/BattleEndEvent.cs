using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBasedFeest.GameEvents;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.BattleEvents
{
    class BattleEndEvent
    {
        int eventTime = 2000;
        int elapsedTime = 0;

        public void Initialize()
        {
        }

        public bool Update(BattleEvent battle, Input input)
        {
            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= eventTime)
            {
                if (battle.aliveActors.TrueForAll(x => x.isPlayer))
                {

                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(BattleEvent battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }
    }
}
