using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedFeest.Actors
{
    class Health
    {
        public int MaxHealth;
        public float CurrentHealth;
        public Color color = Color.White;

        private Rectangle HealthBar;
        private Texture2D texture;

        public Health(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            // TODO: do not hardcode size
            HealthBar = new Rectangle(0, 0, (int)(CurrentHealth / MaxHealth * 100), 20);
            texture = TextureFactory.Instance.GetTexture("health");
        }

        public void Update()
        {
            HealthBar.Width = (int)(CurrentHealth / (float)MaxHealth * 100);
        }

        public void Draw(SpriteBatch spritebatch, Vector2 position)
        {
            HealthBar.X = (int) position.X;
            HealthBar.Y = (int)position.Y;
            spritebatch.Draw(texture, HealthBar, color);
        }

    }
}
