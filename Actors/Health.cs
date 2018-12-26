using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedFeest.Actors
{

    class Health
    {
        public int MaxHealth;
        public float CurrentHealth;

        public bool Shake = false;

        private Color color = Color.White;
        private Rectangle HealthBar;
        private Texture2D texture;

        public Health(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            // TODO: do not hardcode size
            HealthBar = new Rectangle(0, 0, (int)((CurrentHealth / MaxHealth) * 200), 10);
            texture = TextureFactory.Instance.GetTexture("health");
        }

        public void Update()
        {

            HealthBar.Width = (int)(CurrentHealth / (float)MaxHealth * 200);
        }
       
        public void Draw(SpriteBatch spritebatch, Vector2 position, SpriteFont font)
        {
            Vector2 pos = new Vector2(position.X, position.Y);

            if (Shake)
            {
                pos.X += Game1.rnd.Next(2) - 1;
                pos.Y += Game1.rnd.Next(2) - 1;
            }

            HealthBar.X = (int) pos.X;
            HealthBar.Y = (int)pos.Y - 50;            

            spritebatch.Draw(texture, HealthBar, color);
            spritebatch.DrawString(font, $"{((int) CurrentHealth).ToString()} / {((int)MaxHealth).ToString()}", pos + new Vector2(0, -70), color);
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }

    }
}
