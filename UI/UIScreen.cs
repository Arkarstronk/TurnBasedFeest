using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.UI
{
    interface UIScreen
    {        
        void Initialize();
        void Update(GameTime game, Input input);
        void Draw(SpriteBatch spritebatch, SpriteFont font);
    }
}
