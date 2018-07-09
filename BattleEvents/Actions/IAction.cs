using TurnBasedFeest.Actors;

namespace TurnBasedFeest.BattleEvents.Actions
{
    interface IAction : IBattleEvent
    {
        void SetActors(Actor source, Actor target);
        bool isPositive();
        string GetName();
    }
}
