using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Graphics;

namespace TurnBasedFeest.Attributes
{
    class AttackMagicAttribute : IAttribute
    {
        int expiration = 1;        
        Actor gifter;
        private CustomSprite sprite;

        public AttackMagicAttribute(Actor gifter)
        {
            this.gifter = gifter;
            this.sprite = CustomSprite.GetSprite("buff");
            this.sprite.SetColor(gifter.Color);
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font, Vector2 position)
        {
            this.sprite.Draw(spritebatch, position.X, position.Y - 30);
        }

        public float GetAddition()
        {
            return 0.0f;
        }

        public float GetMultiplier()
        {
            return 1.2f;
        }

        public int GetExpiration()
        {
            return expiration;
        }

        public StatisticAttribute getStatistic()
        {
            return StatisticAttribute.ATTACK_MAGIC;
        }
    }
}
