using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.BattleEvents
{
    interface ITurnEvent
    {
        void Initialize();
        void Update(BattleContainer battle, GameTime gameTime, Input input);
        void Draw(BattleContainer battle, SpriteBatch spritebatch, SpriteFont font);
        bool HasCompleted();
    }
}
