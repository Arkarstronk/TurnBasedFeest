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
        void Initialize(Actor source, Actor target);
        IActionResult Update();        
        string GetName();
    }

    interface IActionResult
    {
        bool IsDone();
    }
}
