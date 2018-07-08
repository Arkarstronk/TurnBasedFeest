using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TurnBasedFeest.Actors;

namespace TurnBasedFeest.Actions
{
    class ActionNothing : IAction
    {
        int elapsedTime;
        Actor source;

        public void Initialize(Actor source, Actor target)
        {
            elapsedTime = 0;
            this.source = source;
            this.source.health.color = Color.AliceBlue;
        }

        public IActionResult Execute()
        {
            elapsedTime += (int) Game1.time.ElapsedGameTime.TotalMilliseconds;
            
            if(elapsedTime > 500)
            {
                source.health.color = Color.White;
                return new ActionResultNothing(true);
            }
            else
            {
                return new ActionResultNothing(false);
            }            
        }

        public string GetName()
        {
            return "Defend";
        }        
    }

    class ActionResultNothing : IActionResult
    {
        private bool done;

        public ActionResultNothing(bool done)
        {
            this.done = done;
        }

        public bool IsDone()
        {
            return done;
        }
    }
}
