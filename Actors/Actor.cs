using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedFeest.Actions;
using TurnBasedFeest.Actors.Behaviours;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.Actors
{
    class Actor
    {
        public string name;
        public Vector2 position;
        public Health health;
        public List<IAction> actions;
        public ITurnBehaviour turnBehaviour; 

        public Actor(string name, Vector2 position, int maxHealth, List<IAction> actions, GraphicsDevice device, ITurnBehaviour turnBehaviour)
        {
            this.name = name;
            this.position = position;
            health = new Health(maxHealth, device);
            this.actions = actions;
            this.turnBehaviour = turnBehaviour;
        }

        public void Update()
        {
            health.Update();   
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            // TODO: do not hardcode offset size
            spritebatch.DrawString(font, name, position + new Vector2(0,-20), Color.White);
            health.Draw(spritebatch, position);
            turnBehaviour.Draw(font, spritebatch);
        }
    }
}
