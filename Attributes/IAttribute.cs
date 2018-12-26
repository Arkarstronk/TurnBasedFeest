using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedFeest.Attributes
{
    public enum attributeType
    {
        EVENT,
        INCOMING,
        OUTGOING
    }
    interface IAttribute
    {
        attributeType GetAttributeType();
        int GetExpiration();
        float GetAddition();
        float GetMultiplier();
        void Draw(SpriteBatch spritebatch, SpriteFont font, Vector2 position);
    }
}
