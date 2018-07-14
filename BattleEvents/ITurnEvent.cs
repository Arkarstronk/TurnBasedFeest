using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.GameEvents.Battle;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.BattleEvents
{
    interface ITurnEvent
    {
        void Initialize();
        bool Update(BattleTurnEvent battle, Input input);
        void Draw(BattleTurnEvent battle, SpriteBatch spritebatch, SpriteFont font);
    }
}
