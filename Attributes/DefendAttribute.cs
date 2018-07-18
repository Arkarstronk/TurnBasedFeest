using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;

namespace TurnBasedFeest.Attributes
{
    class DefendAttribute : IAttribute
    {
        int expiration = 1;
        Rectangle rectangle = new Rectangle(0,0,10,10);
        Texture2D icon = TextureFactory.Instance.GetTexture("health");
        Actor gifter;

        public DefendAttribute(Actor gifter)
        {
            this.gifter = gifter;
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font, Vector2 position)
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y - 30;
            spritebatch.Draw(icon, rectangle , gifter.color);
        }

        public float GetDamageMultiplier()
        {
            return 0.25f;
        }

        public int GetExpiration()
        {
            return expiration;
        }

        attributeType IAttribute.GetAttributeType()
        {
            return attributeType.INCOMING;
        }
    }
}
