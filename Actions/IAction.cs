using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBasedFeest.Actors;

namespace TurnBasedFeest.Actions
{
    interface IAction
    {
        /// <summary>
        /// Execute the action,
        /// Returns IActionResult that contains relevant data regarding the executed action.
        /// </summary>
        /// <returns></returns>
        IActionResult Execute(Actor source, Actor target);

        // Needed methods for action animations.
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        string GetName();
    }

    interface IActionResult
    {

    }
}
