using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBasedFeest.Actions;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.Actors.Behaviours
{
    interface ITurnBehaviour
    {
        void Initialize();
        bool DetermineBehaviour(Input input, List<Actor> actors, Actor currentActor);
        ITurnResult GetTurnResult();
        void Draw(SpriteFont font, SpriteBatch spritebatch);
    }

    interface ITurnResult
    {
        void Initialize(Actor source);
        IActionResult Preform();
    }
}
