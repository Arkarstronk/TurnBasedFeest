using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBasedFeest.Actors;
using TurnBasedFeest.BattleEvents;

namespace TurnBasedFeest.Utilities
{
    interface IAnimation
    {
        int AnimationTime();
        double Elapsed();
        bool IsComplete();

        void Update(GameTime gameTime);
    }
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

    class AnimationTimer : IAnimation
    {
        private double elapsed;
        private int animationTime;

        public AnimationTimer(int animationTime)
        {
            this.elapsed = 0;
            this.animationTime = animationTime;
        }

        public int AnimationTime() => animationTime;
        public double Elapsed() => elapsed;
        public bool IsComplete() => elapsed >= AnimationTime();
        

        public void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
        }
    }

    class AnimationHealthChange: IAnimation
    {

        private double elapsed = 0;
        private Actor actor;
        private float beginHp;
        private int damage;

        public AnimationHealthChange(Actor actor, float beginHp, int damage)
        {
            this.actor = actor;
            this.beginHp = beginHp;
            this.damage = damage;

            this.actor.Health.Shake = true;
            this.actor.Health.SetColor(damage >= 0 ? Color.Red : Color.Green);
            
        }

        public double Elapsed() => elapsed;
        public int AnimationTime() => 1200;
        public void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            var damageSubtractionStep = damage * gameTime.ElapsedGameTime.TotalMilliseconds / 1200.0;

            actor.Health.CurrentHealth -= (float)damageSubtractionStep;
            actor.Health.CurrentHealth = Math.Max(0, actor.Health.CurrentHealth);
            actor.Health.CurrentHealth = Math.Min(actor.Health.CurrentHealth, actor.Health.MaxHealth);

            if (IsComplete())
            {
                actor.Health.CurrentHealth = Math.Max(0, beginHp - damage);
                actor.Health.CurrentHealth = Math.Min(actor.Health.CurrentHealth, actor.Health.MaxHealth);

                this.actor.Health.Shake = false;
                this.actor.Health.SetColor(Color.White);
            }
        }
        public bool IsComplete() => elapsed >= AnimationTime();
    }
}
