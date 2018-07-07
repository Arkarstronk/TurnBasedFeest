using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedFeest.Entities
{
    class Health
    {
        public int entityCurrentHealth;
        public int entityMaxHealth;
        private Rectangle entityHealthBar;
        private Texture2D entityHealthBarTex;

        public Health(int maxHealth, GraphicsDevice device)
        {
            entityMaxHealth = maxHealth;
            entityCurrentHealth = maxHealth;
            // TODO: do not hardcode size 
            entityHealthBar = new Rectangle(0, 0, (int)(entityCurrentHealth / entityMaxHealth * 100), 20);
            entityHealthBarTex = new Texture2D(device, 1, 1);
            entityHealthBarTex.SetData(new[] { Color.White });
        }

        public void Update()
        {
            entityHealthBar.Width = (int)(entityCurrentHealth / (float)entityMaxHealth * 100);
        }

        public void Draw(SpriteBatch spritebatch, Vector2 position)
        {
            entityHealthBar.X = (int) position.X;
            entityHealthBar.Y = (int)position.Y;
            spritebatch.Draw(entityHealthBarTex, entityHealthBar, Color.White);
        }

    }
}
