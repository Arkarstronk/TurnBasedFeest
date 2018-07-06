using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedFeest.Entities
{
    class Entity
    {
        public string name;
        private Vector2 position;

        public int entityCurrentHealth;
        public int entityMaxHealth;
        private Rectangle entityHealthBar;
        private Texture2D entityHealthBarTex;

        public bool moveRemaining;

        public void Initialize(string name, Vector2 position, int maxHealth, GraphicsDevice device)
        {
            this.name = name;
            this.position = position;

            entityMaxHealth = maxHealth;
            entityCurrentHealth = maxHealth;
            // TODO: do not hardcode size 
            entityHealthBar = new Rectangle((int) this.position.X, (int) this.position.Y, (int)(entityCurrentHealth / entityMaxHealth * 100), 20);
            entityHealthBarTex = new Texture2D(device, 1, 1);
            entityHealthBarTex.SetData(new[] { Color.White });

            moveRemaining = true;
        }

        public void Update()
        {
            entityHealthBar.Width = (int)(entityCurrentHealth / (float)entityMaxHealth * 100);
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            // TODO: do not hardcode offset size
            spritebatch.DrawString(font, name, position + new Vector2(0,-20), Color.White);
            spritebatch.Draw(entityHealthBarTex, entityHealthBar, Color.White);
        }
    }
}
