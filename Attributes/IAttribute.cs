using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;

namespace TurnBasedFeest.Attributes
{
    interface IAttribute
    {        
        int GetExpiration();
        float GetAddition();
        float GetMultiplier();
        StatisticAttribute getStatistic();
        void Draw(SpriteBatch spritebatch, SpriteFont font, Vector2 position);
    }
}
