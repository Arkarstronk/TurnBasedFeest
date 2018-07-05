using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedFeest.Entities
{
    class Player
    {
        public int PlayerCurrentHealth;
        public int PlayerMaxHealth;
        private Rectangle PlayerHealthBar;
        private Texture2D PlayerHealthBarTex;        

        public void Initialize(int maxHealth, GraphicsDevice device)
        {
            PlayerMaxHealth = maxHealth;
            PlayerCurrentHealth = maxHealth;
            // TODO: do not hardcode position and size 
            PlayerHealthBar = new Rectangle(120, 200, (int)(PlayerCurrentHealth/PlayerMaxHealth * 100), 20);

            PlayerHealthBarTex = new Texture2D(device, 1, 1);
            PlayerHealthBarTex.SetData(new[] { Color.White });
        }

        public void Update()
        {
            PlayerHealthBar.Width = (int)(PlayerCurrentHealth / PlayerMaxHealth * 100);
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            // TODO: do not hardcode position
            spritebatch.DrawString(font, "Player", new Vector2(120, 180), Color.White);
            spritebatch.Draw(PlayerHealthBarTex, PlayerHealthBar, Color.White);
        }
    }
}
