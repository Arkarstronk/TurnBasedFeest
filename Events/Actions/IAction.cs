
using TurnBasedFeest.Actors;
using TurnBasedFeest.Events;

namespace TurnBasedFeest.Events.Actions
{
    interface IAction : IBattleEvent
    {
        void SetActors(Actor source, Actor target);
        bool isPositive();
        string GetName();
    }
}
