using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedFeest.Utilities
{
    class AnimationHelper
    {
        private double animationTime;
        private double elapsedTime;
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

            action(MathHelper.SmoothStep(0, 1, (float)(elapsedTime/animationTime)));
        }

        public bool HasCompleted() => elapsedTime >= animationTime;

    }
}
