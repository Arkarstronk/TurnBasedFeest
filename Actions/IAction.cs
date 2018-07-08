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
        IActionResult Execute();

        void Initialize(Actor source, Actor target);
        
        string GetName();
    }

    interface IActionResult
    {
        bool IsDone();
    }
}
