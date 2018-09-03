using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBasedFeest.BattleEvents;

namespace TurnBasedFeest.Utilities
{
    class AnimationHelper
    {
        protected double animationTime;
        protected double elapsedTime;
        private Action<double> action;

        public AnimationHelper(double animationTime, Action<double> action)
        {
            this.animationTime = animationTime;
            this.elapsedTime = 0.0;
            this.action = action;
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            action?.Invoke(MathHelper.SmoothStep(0, 1, (float)(elapsedTime / animationTime)));
        }

        public bool HasCompleted() => elapsedTime >= animationTime;
    }

    // The animation where health jumps up
    class HealthAnimation : AnimationHelper
    {
        private int health;

        public HealthAnimation(double animationTime, int health) : base(animationTime, null)
        {
            this.health = health;
        }

        public void Draw(SpriteBatch batch, SpriteFont font, Vector2 position)
        {
            batch.DrawString(font, health.ToString(), position + new Vector2(0, -(float)(elapsedTime / animationTime) * 50 + 20), Color.White, 0, new Vector2(), 2, SpriteEffects.None, 1);
        }
    }
}
