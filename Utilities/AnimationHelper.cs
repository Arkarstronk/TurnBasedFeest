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
}
