using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedFeest.Entities
{
    class Entity
    {
        public string name;
        private Vector2 position;
        public Health health;
        public bool moveRemaining;

        public Entity(string name, Vector2 position, int maxHealth, GraphicsDevice device)
        {
            this.name = name;
            this.position = position;
            health = new Health(maxHealth, device);
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
