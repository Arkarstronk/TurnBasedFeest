using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.GameEvents.Battle
{
    interface BattleEvent
    {
        void Initialize(BattleContainer battle);
        void Draw(SpriteBatch batch, SpriteFont font);
        void Update(GameTime gameTime, Input input);

        bool HasCompleted();
    }
}
