using TurnBasedFeest.Actors;

namespace TurnBasedFeest.BattleEvents.Actions
{
    interface IAction : ITurnEvent
    {
        void SetActors(Actor source, params Actor[] targets);
        bool IsSupportive();
        string GetName();
    }
}
