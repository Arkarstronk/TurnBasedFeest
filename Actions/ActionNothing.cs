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

        public IActionResult Execute(Actor source, Actor target)
        {            
            // Add extra defense to this target or something.
            return new ActionResultNothing();
        }

        public void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Defend";
        }
    }

    class ActionResultNothing : IActionResult{}
}
