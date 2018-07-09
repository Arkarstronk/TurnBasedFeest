using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.GameEvents;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.BattleEvents
{
    interface IBattleEvent
    {
        void Initialize();
        bool Update(BattleEvent battle, Input input);
        void Draw(BattleEvent battle, SpriteBatch spritebatch, SpriteFont font);
    }
}
