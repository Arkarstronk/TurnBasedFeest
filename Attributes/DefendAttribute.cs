using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TurnBasedFeest.Attributes
{
    class DefendAttribute : IAttribute
    {
        int expiration = 2;
        Rectangle rectangle = new Rectangle(0,0,10,10);
        Texture2D icon = TextureFactory.Instance.GetTexture("health");

        public void Draw(SpriteBatch spritebatch, SpriteFont font, Vector2 position)
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y - 30;
            spritebatch.Draw(icon, rectangle , Color.Violet);
        }

        public float GetDamageMultiplier()
        {
            return 0.25f;
        }

        public int GetExpiration()
        {
            expiration -= 1;
            return expiration;
        }

        attributeType IAttribute.GetAttributeType()
        {
            return attributeType.INCOMING;
        }
    }
}
