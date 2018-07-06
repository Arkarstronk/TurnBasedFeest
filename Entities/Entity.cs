using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedFeest.Entities
{
    class Entity
    {
        public int EntityCurrentHealth;
        public int EntityMaxHealth;
        private Rectangle EntityHealthBar;
        private Texture2D EntityHealthBarTex;        

        public void Initialize(int maxHealth, GraphicsDevice device)
        {
            EntityMaxHealth = maxHealth;
            EntityCurrentHealth = maxHealth;
            // TODO: do not hardcode position and size 
            EntityHealthBar = new Rectangle(120, 200, (int)(EntityCurrentHealth/EntityMaxHealth * 100), 20);

            EntityHealthBarTex = new Texture2D(device, 1, 1);
            EntityHealthBarTex.SetData(new[] { Color.White });
        }

        public void Update()
        {
            EntityHealthBar.Width = (int)(EntityCurrentHealth / EntityMaxHealth * 100);
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            // TODO: do not hardcode position
            spritebatch.DrawString(font, "Player", new Vector2(120, 180), Color.White);
            spritebatch.Draw(EntityHealthBarTex, EntityHealthBar, Color.White);
        }
    }
}
