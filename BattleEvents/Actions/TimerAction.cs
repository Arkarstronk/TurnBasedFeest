using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedFeest.Actors;
using TurnBasedFeest.Utilities;

namespace TurnBasedFeest.BattleEvents.Actions
{
    class TimerAction: IAction
    {
        private double animationTime;
        private double elapsedTime;

        public TimerAction(int milliseconds)
        {
            this.animationTime = milliseconds;
        }

        public void Draw(BattleContainer battle, SpriteBatch spritebatch, SpriteFont font)
        {
        }

        public string GetName() => "";
        public bool HasCompleted() => elapsedTime >= animationTime;
        public bool IsSupportive() => true;
        public void SetActors(Actor source, params Actor[] targets) { }

        public void Initialize()
        {
            elapsedTime = 0;
        }

        public void Update(BattleContainer battle, GameTime gameTime, Input input)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        }
    }
}
