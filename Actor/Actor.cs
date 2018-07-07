using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedFeest.Actions;

namespace TurnBasedFeest.Actors
{
    class Actor
    {
        public string name;
        public Vector2 position;
        public Health health;
        public bool moveRemaining;
        public List<IAction> actions;

        public Actor(string name, Vector2 position, int maxHealth, List<IAction> actions, GraphicsDevice device)
        {
            this.name = name;
            this.position = position;
            health = new Health(maxHealth, device);
            this.actions = actions;
            moveRemaining = true;
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
        }
    }
}
