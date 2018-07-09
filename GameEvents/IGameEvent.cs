using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.GameEvents
{
    interface IGameEvent
    {
        void Initialize(List<Actor> actors);
        bool Update(Game1 game, Input input);
        void Draw(SpriteBatch spritebatch, SpriteFont font);
    }
}
