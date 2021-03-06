﻿using TurnBasedFeest.Actors;

namespace TurnBasedFeest.BattleEvents.Actions
{
    interface IAction : ITurnEvent
    {
        void SetActors(Actor source, Actor target);
        bool IsSupportive();
        string GetName();
    }
}
