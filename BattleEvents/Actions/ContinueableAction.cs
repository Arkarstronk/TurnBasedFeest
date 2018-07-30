using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBasedFeest.GameEvents.Battle;

namespace TurnBasedFeest.BattleEvents.Actions
{
    interface ContinueableAction
    {
        bool HasNextEvent();
        ITurnEvent NextEvent();        
    }
}
