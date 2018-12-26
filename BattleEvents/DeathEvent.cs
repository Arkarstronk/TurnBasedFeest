using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Utilities;
using TurnBasedFeest.Actors;
using Microsoft.Xna.Framework;

namespace TurnBasedFeest.BattleEvents
{
    class DeathEvent : ITurnEvent
    {
        int eventTime = 2000;
        int elapsedTime;
        Actor deceased;

        public DeathEvent(Actor deceased)
        {
            this.deceased = deceased;
        }

        public void Initialize()
        {
            elapsedTime = 0;
        }

        public void Update(BattleContainer battle, GameTime gameTime, Input input)
        {
            battle.PushSplashText($"{deceased.Name} is critically wounded.");

            elapsedTime += (int)Game1.time.ElapsedGameTime.TotalMilliseconds;

            if (HasCompleted())
            {
                deceased.HandedOutAttributes.ForEach(x => x.receiver.Attributes.Remove(x.attribute));
            }
        }

        public void Draw(BattleContainer battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }

        public bool HasCompleted()
        {
            return elapsedTime >= eventTime;
        }
    }
}
