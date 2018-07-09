using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedFeest.Actors
{
    class Health
    {
        public float actorCurrentHealth;
        public int actorMaxHealth;
        private Rectangle actorHealthBar;
        private Texture2D actorHealthBarTex;
        public Color color = Color.White;

        public Health(int maxHealth, GraphicsDevice device)
        {
            actorMaxHealth = maxHealth;
            actorCurrentHealth = maxHealth;
            // TODO: do not hardcode size 
            actorHealthBar = new Rectangle(0, 0, (int)(actorCurrentHealth / actorMaxHealth * 100), 20);
            actorHealthBarTex = new Texture2D(device, 1, 1);
            actorHealthBarTex.SetData(new[] { Color.White });
        }

        public void Update()
        {
            actorHealthBar.Width = (int)(actorCurrentHealth / (float)actorMaxHealth * 100);
        }

        public void Draw(SpriteBatch spritebatch, Vector2 position)
        {
            actorHealthBar.X = (int) position.X;
            actorHealthBar.Y = (int)position.Y;
            spritebatch.Draw(actorHealthBarTex, actorHealthBar, color);
        }

    }
}
