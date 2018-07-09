using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.BattleSystem;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.Events
{
    interface IBattleEvent
    {
        void Initialize();
        bool Update(Battle battle, Input input);
        void Draw(Battle battle, SpriteBatch spritebatch, SpriteFont font);
    }
}
