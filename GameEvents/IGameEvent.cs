using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.GameEvents
{
    interface IGameEvent
    {
        void Initialize(List<Actor> actors);        
        void Draw(SpriteBatch spritebatch, SpriteFont font);
        void Update(GameTime game, Input input);
        bool HasCompleted();
    }
}
